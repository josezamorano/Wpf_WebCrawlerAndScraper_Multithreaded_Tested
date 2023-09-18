using ServiceLayer.Enumerations;
using System;

namespace DataAccessLayer.IOFiles.Models
{
    public class CsvDataFormat
    {
        public string FullyQualifiedUrl { get; set; }

        public Uri? AbsoluteLink { get; set; }

        public string AbsoluteLinkStringFormatted { get; set; }

        public LinkStatus? LinkStatus { get; set; }

        public bool? IsLinkFullyQualified { get; set; }

        public int? Level { get; set; }

        public string OriginalLink { get; set; }

        public string ParentPageUrl { get; set; }

        public string SavedFullFileName { get; set; }
        public WebSitePageStatus? WebSitePageStatus { get; set; }

    }
}
