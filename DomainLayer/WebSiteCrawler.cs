using DataAccessLayer.IOFiles.Models;
using DataAccessLayer.Utils.Interfaces;
using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using ServiceLayer.Enumerations;
using ServiceLayer.Interfaces;
using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using static ServiceLayer.DelegateTypes.CustomDelegate;

namespace DomainLayer
{
    public class WebSiteCrawler : IWebSiteCrawler
    {
        private static readonly SemaphoreSlim _semaphore1 = new SemaphoreSlim(1, 1);
        private bool _crawllAllPagesIsActive = false;
        private int _websitePagesCount;
        private int _totalPagesToSearch;
        private static int INITIAL_PAGE_LEVEL_TO_SEARCH = 0;
        private const string LINKS_POSITIVE_CRAWL_REPORT = "PositiveCrawlReport";
        private const string LINKS_FAILED_CRAWL_REPORT = "FailedCrawlReport";
        private const string LINKS_PENDING_CRAWL_REPORT = "PendingCrawlReport";
        private string _positiveCrawlReportFullPath;
        private string _failedCrawlReportFullPath;
        private string _pendingCrawlReportFullPath;

        private readonly Queue<CrawlJob> _jobQueue;
        SortedDictionary<string, CrawledLinkInfo> _crawlPositiveResults;
        SortedDictionary<string, CrawledLinkInfo> _crawlFailedResults;
        CrawlExecutionReport _crawlExecutionReport;

        IHtmlParser _htmlParser;
        ILinkInspector _linkInspector;
        IDownloadProvider _downloadProvider;
        IFileActivityManager _fileActivityManager;

        public WebSiteCrawler(
            IHtmlParser htmlParser,
            ILinkInspector linkInspector,
            IDownloadProvider downloadProvider,
            IFileActivityManager fileActivityManager
            )
        {
            _jobQueue = new Queue<CrawlJob>();
            _crawlPositiveResults = new SortedDictionary<string, CrawledLinkInfo>();
            _crawlFailedResults = new SortedDictionary<string, CrawledLinkInfo>();
            _crawlExecutionReport = new CrawlExecutionReport();
            _htmlParser = htmlParser;
            _linkInspector = linkInspector;
            _downloadProvider = downloadProvider;
            _fileActivityManager = fileActivityManager;
        }

        //Tested
        public void RunCrawlerMultithreaded(WebCrawlerInfo webCrawlerInfo)
        {
            SetInitialReportStatus(webCrawlerInfo.Uri, webCrawlerInfo.TotalPagesToSearch);
            CreateCSVFilelReports(webCrawlerInfo.ReportFolderFullPath);
            ExecuteCrawlerMultithreaded(webCrawlerInfo.ReportFolderFullPath, webCrawlerInfo.CrawlReportCallback);
        }

        #region Private Methods

        private void SetInitialReportStatus(Uri url, int maxPagesToSearch)
        {
            _jobQueue.Clear();
            CrawledLinkInfo initialCrawledLinkInfo = new CrawledLinkInfo()
            {
                AbsoluteLink = url,
                IsLinkFullyQualified = true,
                Level = INITIAL_PAGE_LEVEL_TO_SEARCH
            };
            _websitePagesCount = 0;
            _crawlPositiveResults.Clear();
            _crawlFailedResults.Clear();
            _crawllAllPagesIsActive = (maxPagesToSearch == 0);

            AddToJobQueue(initialCrawledLinkInfo);
            _totalPagesToSearch = (maxPagesToSearch == 0) ? _jobQueue.Count : maxPagesToSearch;
        }

        private void CreateCSVFilelReports(string reportFolderFullPath)
        {
            _positiveCrawlReportFullPath = _fileActivityManager.CreateFullPathFileName(LINKS_POSITIVE_CRAWL_REPORT, reportFolderFullPath, FileExtension.csv);
            _failedCrawlReportFullPath = _fileActivityManager.CreateFullPathFileName(LINKS_FAILED_CRAWL_REPORT, reportFolderFullPath, FileExtension.csv);
            _pendingCrawlReportFullPath = _fileActivityManager.CreateFullPathFileName(LINKS_PENDING_CRAWL_REPORT, reportFolderFullPath, FileExtension.csv);
            _fileActivityManager.CreateHeaderInCsvFile(_positiveCrawlReportFullPath);
            _fileActivityManager.CreateHeaderInCsvFile(_failedCrawlReportFullPath);
            _fileActivityManager.CreateHeaderInCsvFile(_pendingCrawlReportFullPath);
        }

        private void ExecuteCrawlerMultithreaded(string reportFolderFullPath, CrawlReportDelegate CrawlReportCallback)
        {
            Thread mainWorkerThread = new Thread(() => {

                List<Thread> threads = new List<Thread>();
                while (_websitePagesCount < _totalPagesToSearch)
                {
                    if (_jobQueue.Count == 0) { continue; }
                    Thread threadCrawler = new Thread(() => {

                        if (_jobQueue.Count == 0) { return; }
                        CrawlJob crawlJobDequeued = GetCrawlJobFromJobQueue();
                        CrawledLinkInfo crawledLinkInfo = GetCrawledLinkInfo(reportFolderFullPath, crawlJobDequeued);
                        if (crawledLinkInfo == null) { return; }
                        List<string> allLinksFoundInPage = _htmlParser.GetLinks(crawledLinkInfo.WebSitePage);
                        if (allLinksFoundInPage == null || allLinksFoundInPage.Count == 0) { return; }
                        ResolveAllLinksFoundInPage(allLinksFoundInPage, crawlJobDequeued);
                    });
                    threadCrawler.Name = "ThreadCrawler_" + threads.Count;
                    threadCrawler.IsBackground = true;
                    threadCrawler.Start();
                    threads.Add(threadCrawler);
                    setWebsitePageCount(threads.Count);
                    //If all threads are not alive and the jobqueue is empty
                    //then set the total pages to search to negative value
                    var allThreadsAreAlive = InspectAllThreadsAreAlive(threads);
                    if (!allThreadsAreAlive)
                    {
                        InspectCrawlAllPagesIsActiveAndJobQueueIsEmpty();
                    }
                }

                InspectAllThreadsAreAlive(threads);
                _crawlExecutionReport.ThreadsUsed = threads;

                _crawlExecutionReport.CrawledLinksPositiveResult = _crawlPositiveResults;
                _crawlExecutionReport.CrawledLinksFailedResult = _crawlFailedResults;
                _crawlExecutionReport.LinksPendingCrawling = GetAllPendingLinksInJobQueue();
                CrawlReportCallback(_crawlExecutionReport);
            });

            mainWorkerThread.Name = "Main_Worker_Thread";
            mainWorkerThread.IsBackground = true;
            mainWorkerThread.Start();
        }

        //Lock
        private CrawledLinkInfo GetCrawledLinkInfo(string reportFolderFullPath, CrawlJob crawlJobDequeued)
        {
            _semaphore1.Wait();
            try
            {
                CrawledLinkInfo crawledLinkInfoCompleted = null;
                bool pageIsDownloaded = false;
                while (!pageIsDownloaded)
                {
                    if (crawlJobDequeued == null) { continue; }
                    if (!_linkInspector.IsUriSchemeValid(crawlJobDequeued.Uri.Scheme)) { continue; }
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    string pageContent = _downloadProvider.DownloadPageUsingSelenium(crawlJobDequeued.Url);
                    crawledLinkInfoCompleted = CreateCrawledLinkInfo(crawlJobDequeued, pageContent);
                    if (!string.IsNullOrEmpty(pageContent))
                    {
                        pageIsDownloaded = ResolveSavingPageContentDataToReports(crawledLinkInfoCompleted, reportFolderFullPath, crawlJobDequeued);
                    }
                    else
                    {
                        AddToFailedResults(crawledLinkInfoCompleted);
                    }
                }

                return crawledLinkInfoCompleted;
            }
            finally { _semaphore1.Release(); }
        }

        private bool ResolveSavingPageContentDataToReports(CrawledLinkInfo crawledLinkInfoCompleted, string reportFolderFullPath, CrawlJob crawlJobDequeued)
        {
            crawledLinkInfoCompleted.WebSitePageFullFileName = _fileActivityManager.CreateFullPathFileName(crawlJobDequeued.Url, reportFolderFullPath, FileExtension.txt);
            bool webSiteContentIsWrittenToTxtFile = _fileActivityManager.WriteToTxtFile(crawledLinkInfoCompleted.WebSitePageFullFileName, crawledLinkInfoCompleted.WebSitePage);
            if (webSiteContentIsWrittenToTxtFile)
            {
                AddToPositiveResults(crawlJobDequeued.Url, crawledLinkInfoCompleted);
                return true;
            }
            else
            {
                AddToFailedResults(crawledLinkInfoCompleted);
                return false;
            }
        }

        private CrawledLinkInfo CreateCrawledLinkInfo(CrawlJob crawlJobDequeued, string pageContent)
        {
            var urlStringFormatted = new Uri(crawlJobDequeued.Url).ToString();
            var crawledLinkInfoCompleted = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(crawlJobDequeued.Url),
                AbsoluteLinkStringFormatted = (urlStringFormatted.EndsWith('/'))
                                              ? (urlStringFormatted.Remove(urlStringFormatted.Length - 1))
                                              : urlStringFormatted,
                LinkStatus = LinkStatus.Valid_ParentPage,
                IsLinkFullyQualified = true,
                Level = crawlJobDequeued.PageNumber,
                OriginalLink = crawlJobDequeued.Url,
                ParentPageUrl = crawlJobDequeued.Url,
                WebSitePage = pageContent,
                WebSitePageStatus = WebSitePageStatus.DownloadComplete
            };

            return crawledLinkInfoCompleted;
        }

        //Lock
        private void ResolveAllLinksFoundInPage(List<string> allLinksFoundInPage, CrawlJob crawlJobDequeued)
        {
            _semaphore1.Wait();
            try
            {
                foreach (var linkFound in allLinksFoundInPage)
                {
                    CrawledLinkInfo crawledLinkInfoFound = new CrawledLinkInfo()
                    {
                        LinkStatus = LinkStatus.Found_ForValidation,
                        IsLinkFullyQualified = false,
                        Level = crawlJobDequeued.PageNumber + 1,
                        OriginalLink = linkFound,
                        ParentPageUrl = crawlJobDequeued.Url,
                        WebSitePage = string.Empty,
                        WebSitePageStatus = WebSitePageStatus.NotDownloaded
                    };
                    CrawledLinkInfo linkResult = _linkInspector.ValidateLink(crawlJobDequeued.Uri.Host, crawledLinkInfoFound);
                    string parentLink = _linkInspector.GetParentUriString(crawlJobDequeued.Uri);
                    crawledLinkInfoFound.AbsoluteLink = (linkResult.IsLinkFullyQualified) ? new Uri(linkFound) : _linkInspector.CreateAbsoluteUriLink(parentLink, linkFound);
                    if (!IsAbsoluteUriValid(crawledLinkInfoFound, crawlJobDequeued)) { continue; }
                    if (linkResult.LinkStatus == LinkStatus.Valid_ParentPage || linkResult.LinkStatus == LinkStatus.Valid_InternalChildPage)
                    {
                        AddToJobQueue(crawledLinkInfoFound);
                    }
                    else
                    {
                        AddToFailedResults(crawledLinkInfoFound);
                    }
                }
            }
            finally { _semaphore1.Release(); }
        }

        //Lock
        private SortedDictionary<string, CrawledLinkInfo> GetAllPendingLinksInJobQueue()
        {
            _semaphore1.Wait();
            try
            {
                SortedDictionary<string, CrawledLinkInfo> _crawlIncompleteResults = new SortedDictionary<string, CrawledLinkInfo>();
                while (_jobQueue.Count > 0)
                {
                    var crawlJobDequeuedPending = _jobQueue.Dequeue();
                    var urlString = crawlJobDequeuedPending.Url.ToString();
                    CrawledLinkInfo link = new CrawledLinkInfo()
                    {
                        AbsoluteLink = new Uri(crawlJobDequeuedPending.Url),
                        AbsoluteLinkStringFormatted = (urlString.EndsWith('/')) ? (urlString.Remove(urlString.Length - 1)) : urlString,
                        LinkStatus = LinkStatus.Found_ForValidation,
                        IsLinkFullyQualified = false,
                        Level = crawlJobDequeuedPending.PageNumber,
                        OriginalLink = crawlJobDequeuedPending.Url,
                        ParentPageUrl = crawlJobDequeuedPending.Url,
                        WebSitePage = string.Empty,
                        WebSitePageStatus = WebSitePageStatus.NotDownloaded
                    };

                    var keyValuePair = new KeyValuePair<string, CrawledLinkInfo>(link.AbsoluteLink.ToString(), link);
                    var record = _fileActivityManager.CreateSingleCsvRecord(keyValuePair);
                    _fileActivityManager.SaveSingleRecordToCSVFile(_pendingCrawlReportFullPath, record);

                    _crawlIncompleteResults.Add(link.AbsoluteLink.ToString(), link);
                }

                return _crawlIncompleteResults;
            }
            finally { _semaphore1.Release(); }
        }

        //Lock
        private void setWebsitePageCount(int pageCount)
        {
            _semaphore1.Wait();
            try
            {
                _websitePagesCount = (_crawllAllPagesIsActive) ? INITIAL_PAGE_LEVEL_TO_SEARCH : pageCount;
            }
            finally { _semaphore1.Release(); }
        }

        //Lock
        private void InspectCrawlAllPagesIsActiveAndJobQueueIsEmpty()
        {
            _semaphore1.Wait();
            try
            {
                if (_jobQueue.Count == 0 && _crawllAllPagesIsActive)
                {
                    _totalPagesToSearch = -1;
                }
            }
            finally { _semaphore1.Release(); }
        }

        private bool InspectAllThreadsAreAlive(List<Thread> threads)
        {
            bool areAlive = true;
            while (areAlive)
            {
                int counter = 0;
                foreach (var thread in threads)
                {
                    if (!thread.IsAlive) { counter++; }
                }

                if (counter == threads.Count)
                {
                    areAlive = false;
                }
            }

            return areAlive;
        }

        private bool IsAbsoluteUriValid(CrawledLinkInfo crawledLinkInfoFound, CrawlJob crawlJobDequeued)
        {
            bool absoluteUrlIsUnique = false;
            bool absoluteUriLinkIsStoredInPositiveResults = false;
            bool absoluteUriLinkIsStoredInFailedResults = false;
            bool absoluteUriLinkJobIsEnqueued = false;

            if (_linkInspector.IsAbsoluteUrlLinkUnique(crawledLinkInfoFound, crawlJobDequeued))
            {
                absoluteUrlIsUnique = true;
            }
            if (_linkInspector.IsAbsoluteUriLinkStoredInPositiveResults(_crawlPositiveResults, crawledLinkInfoFound))
            {
                absoluteUriLinkIsStoredInPositiveResults = true;
            }
            if (_linkInspector.IsAbsoluteUriLinkStoredInFailedResults(_crawlFailedResults, crawledLinkInfoFound))
            {
                absoluteUriLinkIsStoredInFailedResults = true;
            }
            if (_linkInspector.IsAbsoluteUriLinkJobEnqueued(_jobQueue, crawledLinkInfoFound))
            {
                absoluteUriLinkJobIsEnqueued = true;
            }

            if (absoluteUrlIsUnique && !absoluteUriLinkIsStoredInPositiveResults && !absoluteUriLinkIsStoredInFailedResults && !absoluteUriLinkJobIsEnqueued)
            {
                return true;
            }

            return false;
        }

        private bool AddToJobQueue(CrawledLinkInfo crawledLinkInfoFound)
        {
            if (!_linkInspector.IsAbsoluteUriLinkStoredInPositiveResults(_crawlPositiveResults, crawledLinkInfoFound))
            {
                CrawlJob newCrawlJob = new CrawlJob(crawledLinkInfoFound.AbsoluteLink.ToString(), crawledLinkInfoFound.Level);
                _jobQueue.Enqueue(newCrawlJob);

                return true;
            }

            return false;
        }

        //Lock
        private CrawlJob GetCrawlJobFromJobQueue()
        {
            _semaphore1.Wait();
            try
            {
                CrawlJob crawlJobDequeued = _jobQueue.Dequeue();

                return crawlJobDequeued;
            }
            finally { _semaphore1.Release(); }
        }

        private bool AddToPositiveResults(string crawlJobDequeuedUrl, CrawledLinkInfo crawledLinkInfoCompleted)
        {
            if (!_linkInspector.IsAbsoluteUriLinkStoredInPositiveResults(_crawlPositiveResults, crawledLinkInfoCompleted))
            {
                SaveSingleCSVRecord(_positiveCrawlReportFullPath, crawlJobDequeuedUrl, crawledLinkInfoCompleted);
                _crawlPositiveResults.Add(crawlJobDequeuedUrl, crawledLinkInfoCompleted);

                return true;
            }

            return false;
        }

        private bool AddToFailedResults(CrawledLinkInfo crawledLinkInfoFound)
        {
            if (!_linkInspector.IsAbsoluteUriLinkStoredInFailedResults(_crawlFailedResults, crawledLinkInfoFound))
            {
                string failedUrl = crawledLinkInfoFound?.AbsoluteLink?.ToString();
                SaveSingleCSVRecord(_failedCrawlReportFullPath, failedUrl, crawledLinkInfoFound);
                if(failedUrl != null)
                {
                    _crawlFailedResults.Add(failedUrl, crawledLinkInfoFound);
                }
                

                return true;
            }

            return false;
        }

        private void SaveSingleCSVRecord(string reportFileFullPath, string crawlJobDequeuedUrl, CrawledLinkInfo crawledLinkInfoCompleted)
        {
            var keyValuePair = new KeyValuePair<string, CrawledLinkInfo>(crawlJobDequeuedUrl, crawledLinkInfoCompleted);
            CsvDataFormat csvRecord = _fileActivityManager.CreateSingleCsvRecord(keyValuePair);
            _fileActivityManager.SaveSingleRecordToCSVFile(reportFileFullPath, csvRecord);
        }

        #endregion Private Methods 
    }
}
