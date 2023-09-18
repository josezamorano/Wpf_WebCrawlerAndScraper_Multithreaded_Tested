using ServiceLayer.Models;

namespace ServiceLayer.DelegateTypes
{
    public class CustomDelegate
    {
        public delegate void PresentationCrawlReportDelegate(string pathFile);

        public delegate void PresentationScrapeReportDelegate(string report);

        public delegate void CrawlReportDelegate(CrawlExecutionReport crawlReport);
    }
}
