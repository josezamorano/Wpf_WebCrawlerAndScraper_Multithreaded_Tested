using ServiceLayer.Models;
using System;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface ILinkInspector
    {
        CrawledLinkInfo ValidateLink(string targetUriHost, CrawledLinkInfo crawledLinkInfo);

        bool IsLinkFullyQualified(string rawLink);

        bool IsUriSchemeValid(string uriScheme);

        string GetParentUriString(Uri parentUri);

        Uri CreateAbsoluteUriLink(string parentUrl, string childUrl);

        bool IsAbsoluteUrlLinkUnique(CrawledLinkInfo crawledLinkInfoFound, CrawlJob crawlJobDequeued);

        bool IsAbsoluteUriLinkStoredInPositiveResults(SortedDictionary<string, CrawledLinkInfo> crawlPositiveResults, CrawledLinkInfo crawledLinkInfoFound);

        bool IsAbsoluteUriLinkStoredInFailedResults(SortedDictionary<string, CrawledLinkInfo> crawlFailedResults, CrawledLinkInfo crawledLinkInfoFound);

        bool IsAbsoluteUriLinkJobEnqueued(Queue<CrawlJob> jobQueue, CrawledLinkInfo crawledLinkInfoFound);
    }
}
