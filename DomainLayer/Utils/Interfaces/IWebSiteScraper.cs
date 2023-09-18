using DomainLayer.Models;

namespace DomainLayer.Utils.Interfaces
{
    public interface IWebSiteScraper
    {
        void RunScraperMultithreaded(WebScraperInfo webScraperInfo);
    }
}
