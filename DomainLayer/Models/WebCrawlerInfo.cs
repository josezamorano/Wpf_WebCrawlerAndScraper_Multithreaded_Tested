using System;
using static ServiceLayer.DelegateTypes.CustomDelegate;

namespace DomainLayer.Models
{
    public class WebCrawlerInfo
    {
        public Uri Uri { get; set; }

        public int TotalPagesToSearch { get; set; }

        public string ReportFolderFullPath { get; set; }
        public PresentationCrawlReportDelegate PresentationReportCallback { get; set; }

        public CrawlReportDelegate CrawlReportCallback { get; set; }


    }
}
