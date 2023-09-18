using ServiceLayer.Enumerations;
using System;

namespace ServiceLayer.Models
{
    public class CrawledLinkInfo
    {
        public Uri? AbsoluteLink { get; set; }

        public string AbsoluteLinkStringFormatted { get; set; }

        public LinkStatus? LinkStatus { get; set; }

        public bool IsLinkFullyQualified { get; set; }

        public int Level { get; set; }

        public string OriginalLink { get; set; }

        public string ParentPageUrl { get; set; }

        public string WebSitePage { get; set; }

        public string WebSitePageFullFileName { get; set; }

        public WebSitePageStatus? WebSitePageStatus { get; set; }

    }
}
