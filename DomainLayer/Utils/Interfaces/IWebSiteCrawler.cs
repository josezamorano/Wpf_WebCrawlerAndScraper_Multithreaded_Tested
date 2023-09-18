using DomainLayer.Models;

namespace DomainLayer.Utils.Interfaces
{
    public interface IWebSiteCrawler
    {
        void RunCrawlerMultithreaded(WebCrawlerInfo webCrawlerInfo);
    }
}
