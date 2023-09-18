using DataAccessLayer.IOFiles;
using DataAccessLayer.Utils.Interfaces;
using DomainLayer;
using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using UnitTestTestWebCrawlerScraper.Mocks;
using static ServiceLayer.DelegateTypes.CustomDelegate;

namespace UnitTestTestWebCrawlerScraper.DomainLayerTests
{
    public class WebSiteScraperTest
    {
        IFileHelper _fileHelper;
        IPathProvider _pathProvider;
        ICsvFileProvider _csvFileProvider;
        ITextFileProvider _textFileProvider;
        IDirectoryProvider _directoryProvider;


        IWebSiteScraper _webSiteScraper;
        public WebSiteScraperTest()
        {
            _fileHelper = new FileHelper();
            _pathProvider = new PathProvider();
            _csvFileProvider = new Mock_CsvFileProvider();
            _textFileProvider = new Mock_TextFileProvider();
            _directoryProvider = new Mock_DirectoryProvider();
            IFileActivityManager _fileActivityManager = new FileActivityManager(_fileHelper, _pathProvider, _csvFileProvider, _textFileProvider, _directoryProvider);
            _webSiteScraper = new WebSiteScraper(_fileActivityManager);
        }

        [Fact]
        public void RunScraperMultithreaded_ValidInputs_ReturnOk()
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
            _webSiteScraper.RunScraperMultithreaded(webScraperInfo);
            //Assert
        }
    }
}
