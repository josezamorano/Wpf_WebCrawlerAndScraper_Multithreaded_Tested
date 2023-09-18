using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using ServiceLayer.Models;

namespace UnitTestTestWebCrawlerScraper.Mocks
{
    public class Mock_WebSiteCrawler : IWebSiteCrawler
    {
        public void RunCrawlerMultithreaded(WebCrawlerInfo webCrawlerInfo)
        {
            MockRunCrawlMultithreaded(webCrawlerInfo);
        }

        private void MockRunCrawlMultithreaded(WebCrawlerInfo webCrawlerInfo)
        {
            Thread t1 = new Thread(() => { });
            Thread t2 = new Thread(() => { });
            Thread t3 = new Thread(() => { });
            var key = "executed crawl";
            CrawledLinkInfo value = new CrawledLinkInfo();

            KeyValuePair<string, CrawledLinkInfo> keyValuePair = new KeyValuePair<string, CrawledLinkInfo>(key, value);

            CrawlExecutionReport crawlExecutionReport = new CrawlExecutionReport()
            {
                ThreadsUsed = new List<Thread>() { t1, t2, t3 },
                CrawledLinksPositiveResult = new SortedDictionary<string, CrawledLinkInfo>() { },
                CrawledLinksFailedResult = new SortedDictionary<string, CrawledLinkInfo>() { },
                LinksPendingCrawling = new SortedDictionary<string, CrawledLinkInfo>() { },
            };
            crawlExecutionReport.CrawledLinksPositiveResult.Add(key, value);
            webCrawlerInfo.PresentationReportCallback(key);
        }
    }
}
