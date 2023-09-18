using DomainLayer.Models;
using System.Collections.Generic;

namespace DomainLayer.Utils.Interfaces
{
    public interface IDataCollectionManager
    {
        void RunCrawler(WebCrawlerInfo webCrawlerInfo);

        List<string> GetAllFilesInSelectedDirectory(string folderFullPath);

        void RunScraper(WebScraperInfo webScraperInfo);
    }
}
