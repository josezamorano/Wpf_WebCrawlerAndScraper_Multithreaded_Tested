using DataAccessLayer.IOFiles;
using DataAccessLayer.Utils.Interfaces;
using DomainLayer;
using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using ServiceLayer;
using ServiceLayer.Interfaces;
using ServiceLayer.Models;
using UnitTestTestWebCrawlerScraper.Mocks;
using static ServiceLayer.DelegateTypes.CustomDelegate;

namespace UnitTestTestWebCrawlerScraper.DomainLayerTests
{
    public class WebSiteCrawlerTest
    {
        IHtmlParser _htmlParser;
        ILinkInspector _linkInspector;
        IWebSiteCrawler _websiteCrawler;
        IDownloadProvider _downloadProvider;
        IFileActivityManager _fileActivityManager;

        IPathProvider _pathProvider;
        IFileHelper _fileHelper;
        IDirectoryProvider _directoryProvider;
        ICsvFileProvider _csvFileProvider;
        ITextFileProvider _textFileProvider;

        public WebSiteCrawlerTest()
        {
            _htmlParser = new HtmlParser();
            _linkInspector = new LinkInspector();
            _downloadProvider = new Mock_DownloadProvider();
            _fileHelper = new FileHelper();
            _pathProvider = new PathProvider();
            _csvFileProvider = new Mock_CsvFileProvider();
            _textFileProvider = new Mock_TextFileProvider();
            _directoryProvider = new DirectoryProvider();
            _fileActivityManager = new FileActivityManager(_fileHelper, _pathProvider, _csvFileProvider, _textFileProvider, _directoryProvider);
            _websiteCrawler = new Mock_WebSiteCrawler();
        }

        [Fact]
        public void Run_3Pages_ProvidingValidInputs_ReturnOk_3Pages()
        {
            //Arrange
            var fullUrl = "https://www.cnn.com";
            Uri url = new Uri(fullUrl);
            var minNumberOfPages = 0;
            var maxNumberOfPages = 3;

            void GetCrawlReport1(CrawlExecutionReport crawlExecutionReport)
            {
                CrawledLinkInfo actualResultValue = crawlExecutionReport.CrawledLinksPositiveResult.Values.FirstOrDefault();

                //Assert
                var all3ThreadsUsed = (crawlExecutionReport.ThreadsUsed.Count == maxNumberOfPages);
                var allPagesDownloaded = (crawlExecutionReport.CrawledLinksPositiveResult.Count);
                var allResourcesApplied = (allPagesDownloaded > minNumberOfPages && allPagesDownloaded <= maxNumberOfPages);
                Assert.True(all3ThreadsUsed);
                Assert.True(allResourcesApplied);

            }

            void GetPresentationReport(string result)
            {
                var info = result;
            }
            CrawlReportDelegate callbackTest1 = new CrawlReportDelegate(GetCrawlReport1);
            PresentationCrawlReportDelegate presentationReportCallback = new PresentationCrawlReportDelegate(GetPresentationReport);
            var webCrawlerInfo = new WebCrawlerInfo()
            {
                Uri = url,
                TotalPagesToSearch = maxNumberOfPages,
                ReportFolderFullPath = "test",
                CrawlReportCallback = callbackTest1,
                PresentationReportCallback = presentationReportCallback
            };
            //Act
            _websiteCrawler.RunCrawlerMultithreaded(webCrawlerInfo);


        }

        [Fact]
        public void Run_DiscoverAllPages_ProvidingValidInputs_ReturnOk_AllFoundPages()
        {
            //Arrange
            string fullUrl = "https://www.cnn.com";
            Uri url = new Uri(fullUrl);
            var minNumberOfPages = 0;
            var maxNumberOfPages = 0;

            void getCrawlReport2(CrawlExecutionReport actualResult)
            {
                CrawledLinkInfo actualResultValue = actualResult.CrawledLinksPositiveResult.Values.FirstOrDefault();

                //Assert
                var allPagesDownloaded = (actualResult.CrawledLinksPositiveResult.Count);
                var allResourcesApplied = (allPagesDownloaded > minNumberOfPages);
                Assert.True(allResourcesApplied);
            }

            CrawlReportDelegate callbackTest2 = new CrawlReportDelegate(getCrawlReport2);

            void GetPresentationReport(string result)
            {
                var info = result;
            }
            PresentationCrawlReportDelegate presentationReportCallback = new PresentationCrawlReportDelegate(GetPresentationReport);

            var webCrawlerInfo = new WebCrawlerInfo()
            {
                Uri = url,
                TotalPagesToSearch = maxNumberOfPages,
                ReportFolderFullPath = "test",
                CrawlReportCallback = callbackTest2,
                PresentationReportCallback = presentationReportCallback
            };
            //Act
            _websiteCrawler.RunCrawlerMultithreaded(webCrawlerInfo);
        }
    }
}