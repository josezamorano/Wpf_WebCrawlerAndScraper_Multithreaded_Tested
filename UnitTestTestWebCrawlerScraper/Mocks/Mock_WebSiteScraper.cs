using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;

namespace UnitTestTestWebCrawlerScraper.Mocks
{
    public class Mock_WebSiteScraper : IWebSiteScraper
    {
        public void RunScraperMultithreaded(WebScraperInfo webScraperInfo)
        {
            string result = "callback called";
            webScraperInfo.ScrapeReportCallback(result);
        }
    }
}
