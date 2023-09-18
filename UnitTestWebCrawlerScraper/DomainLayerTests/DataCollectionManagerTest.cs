using DataAccessLayer.IOFiles;
using DataAccessLayer.Utils.Interfaces;
using DomainLayer;
using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using ServiceLayer.Models;
using UnitTestTestWebCrawlerScraper.Mocks;
using static ServiceLayer.DelegateTypes.CustomDelegate;

namespace UnitTestTestWebCrawlerScraper.DomainLayerTests
{
    public class DataCollectionManagerTest
    {
        IDataCollectionManager _dataCollectionManager;
        public DataCollectionManagerTest()
        {
            IFileHelper _fileHelper = new FileHelper();
            IPathProvider _pathProvider = new PathProvider();
            ICsvFileProvider _csvFileProvider = new Mock_CsvFileProvider();
            ITextFileProvider _textFileProvider = new Mock_TextFileProvider();
            IDirectoryProvider _directoryProvider = new Mock_DirectoryProvider();
            IFileActivityManager _fileActivityManager = new FileActivityManager(_fileHelper, _pathProvider, _csvFileProvider, _textFileProvider, _directoryProvider);

            IWebSiteCrawler _webSiteCrawler = new Mock_WebSiteCrawler();
            IWebSiteScraper _webSiteScraper = new Mock_WebSiteScraper();

            _dataCollectionManager = new DataCollectionManager(_webSiteCrawler, _webSiteScraper, _fileActivityManager);
        }

        [Fact]
        public void RunCrawler_ValidInputs_Return_Ok()
        {
            void GetPresentationReport(string actualResult)
            {
                var expectedSubstring = "executed crawl";
                Assert.Contains(expectedSubstring, actualResult);
            }

            void GetCrawlRepor(CrawlExecutionReport crawlExecutionReport)
            {
                var info = crawlExecutionReport.ThreadsUsed.Count();
            }

            CrawlReportDelegate crawlReportCallbackTest = new CrawlReportDelegate(GetCrawlRepor);
            PresentationCrawlReportDelegate presentationReportCallbackTeSt = new PresentationCrawlReportDelegate(GetPresentationReport);

            //Arrange
            WebCrawlerInfo webCrawlerInfo = new WebCrawlerInfo()
            {
                Uri = new Uri("https://www.cnn.com"),
                TotalPagesToSearch = 1,
                ReportFolderFullPath = @"c:\test\reports",
                CrawlReportCallback = crawlReportCallbackTest,
                PresentationReportCallback = presentationReportCallbackTeSt
            };
            //Act
            _dataCollectionManager.RunCrawler(webCrawlerInfo);
        }

        [Fact]
        public void GetAllFilesInSelectedDirectory_ValidInputs_Returns_Ok()
        {
            //Arrange
            string folderFullPath = string.Empty;
            //Act
            var actualResult = _dataCollectionManager.GetAllFilesInSelectedDirectory(folderFullPath);
            //Assert
            Assert.True(actualResult.Count > 0); ;
        }

        [Fact]
        public void RunScraper_ValidInputs_ReturnsOk()
        {
            void GetReport(string report)
            {
                var info = report;
                Assert.True(info.Length > 0);
            }

            PresentationScrapeReportDelegate reportCallback = new PresentationScrapeReportDelegate(GetReport);
            //Arrange
            WebScraperInfo webScraperInfo = new WebScraperInfo()
            {
                AllTextFilesToScrape = new Queue<string>(),
                ScrapingFolderFullPath = string.Empty,
                XPathExpression = string.Empty,
                ScrapeReportCallback = reportCallback
            };
            //Act
            _dataCollectionManager.RunScraper(webScraperInfo);
        }
    }
}
