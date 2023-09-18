using ServiceLayer.Enumerations;
using ServiceLayer.Interfaces;
using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ServiceLayer
{
    public class LinkInspector : ILinkInspector
    {
        private static readonly SemaphoreSlim _semaphore3 = new SemaphoreSlim(1, 1);

        //Tested
        public CrawledLinkInfo ValidateLink(string targetUriHost, CrawledLinkInfo crawledLinkInfo)
        {
            _semaphore3.Wait();
            try
            {
                //We Assume that the link is valid
                crawledLinkInfo.LinkStatus = LinkStatus.Valid_InternalChildPage;

                string rawLink = (string.IsNullOrEmpty(crawledLinkInfo.OriginalLink)) ? string.Empty : crawledLinkInfo.OriginalLink.Trim().ToLower().Trim('\n');
                crawledLinkInfo.IsLinkFullyQualified = IsLinkFullyQualified(rawLink);

                if (string.IsNullOrEmpty(rawLink))
                {
                    crawledLinkInfo.LinkStatus = LinkStatus.Invalid_Null;
                    return crawledLinkInfo;
                }

                var linkfragments = rawLink.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (!linkfragments.Any())
                {
                    crawledLinkInfo.LinkStatus = LinkStatus.Invalid_Null;
                    return crawledLinkInfo;
                }

                if (rawLink.StartsWith("mailto"))
                {
                    crawledLinkInfo.LinkStatus = LinkStatus.Invalid_Mailto;
                    return crawledLinkInfo;
                }

                if (rawLink.StartsWith("tel:"))
                {
                    crawledLinkInfo.LinkStatus = LinkStatus.Invalid_Phone;
                    return crawledLinkInfo;
                }

                if (rawLink.StartsWith("sms:"))
                {
                    crawledLinkInfo.LinkStatus = LinkStatus.Invalid_Sms;
                    return crawledLinkInfo;
                }

                Uri absoluteUri;
                bool uriIsCreated = Uri.TryCreate(rawLink, UriKind.Absolute, out absoluteUri);
                if (uriIsCreated)
                {
                    crawledLinkInfo.LinkStatus = (absoluteUri.Host != targetUriHost) ? LinkStatus.Invalid_External : LinkStatus.Valid_InternalChildPage;
                }
                return crawledLinkInfo;
            }
            finally
            {
                _semaphore3.Release();
            }
        }

        //Tested
        public bool IsLinkFullyQualified(string rawLink)
        {
            if (string.IsNullOrEmpty(rawLink)) { return false; }

            if (rawLink.ToLower().StartsWith("http") || rawLink.ToLower().StartsWith("https"))
            {
                Uri absoluteUri;
                bool uriIsCreated = Uri.TryCreate(rawLink, UriKind.Absolute, out absoluteUri);
                if (uriIsCreated)
                {
                    return true;
                }
            }

            return false;
        }

        //Tested
        public bool IsUriSchemeValid(string uriScheme)
        {
            _semaphore3.Wait();
            try
            {
                if (string.IsNullOrEmpty(uriScheme)) { return false; }
                var result = ((uriScheme.ToLower() != "http") && (uriScheme.ToLower() != "https")) ? false : true;
                return result;
            }
            finally { _semaphore3.Release(); }

        }

        //Tested
        public string GetParentUriString(Uri parentUri)
        {
            _semaphore3.Wait();
            try
            {
                if (parentUri == null) { return string.Empty; }
                var builder = new UriBuilder();
                builder.Scheme = parentUri.Scheme;
                builder.Port = parentUri.Port;
                builder.Host = parentUri.Host;
                builder.Path = parentUri.AbsolutePath;
                var parentUriWithoutQuery = builder.Uri;
                return parentUriWithoutQuery.AbsoluteUri
                       .Remove(parentUriWithoutQuery.AbsoluteUri.Length - parentUriWithoutQuery.Segments.Last().Length);
            }
            finally { _semaphore3.Release(); }
        }

        //Tested
        public Uri CreateAbsoluteUriLink(string parentUrl, string childUrl)
        {
            _semaphore3.Wait();
            try
            {
                if (string.IsNullOrEmpty(parentUrl) || string.IsNullOrEmpty(childUrl)) { return null; }
                var parentUri = new Uri(parentUrl);
                var frags = new List<string>();
                if (!string.IsNullOrEmpty(parentUri.PathAndQuery))
                {
                    frags.AddRange(parentUri.PathAndQuery.Split('/'));
                }
                frags.AddRange(childUrl.Split('/'));
                var nonEmptyFrags = frags.Where(s => !string.IsNullOrEmpty(s)).ToList();

                var newChildUrl = string.Join("/", nonEmptyFrags);

                var builder = new UriBuilder();
                builder.Scheme = parentUri.Scheme;
                builder.Port = parentUri.Port;
                builder.Host = parentUri.Host;

                return new Uri(builder.Uri, newChildUrl);
            }
            finally
            {
                _semaphore3.Release();
            }
        }

        //Tested
        public bool IsAbsoluteUrlLinkUnique(CrawledLinkInfo crawledLinkInfoFound, CrawlJob crawlJobDequeued)
        {
            _semaphore3.Wait();
            try
            {
                //Case1: Uri originally crawled and Resulted uri from crawl process are the same.
                if ((crawledLinkInfoFound.AbsoluteLink != null) &&
                      (crawledLinkInfoFound.AbsoluteLink.Host.ToLower() == crawlJobDequeued.Uri.Host.ToLower()) &&
                      (crawledLinkInfoFound.AbsoluteLink.PathAndQuery.ToLower() == crawlJobDequeued.Uri.PathAndQuery.ToLower())
                     )
                {
                    return false;
                }

                return true;
            }
            finally
            {
                _semaphore3.Release();
            }
        }

        //Tested
        public bool IsAbsoluteUriLinkStoredInPositiveResults(SortedDictionary<string, CrawledLinkInfo> crawlPositiveResults, CrawledLinkInfo crawledLinkInfoFound)
        {
            _semaphore3.Wait();
            try
            {
                //Case1: The link as already been stored in the Positive Results
                if (crawledLinkInfoFound.AbsoluteLink != null && crawlPositiveResults.ContainsKey(crawledLinkInfoFound.AbsoluteLink.ToString()))
                {
                    return true;
                }
                return false;
            }
            finally { _semaphore3.Release(); }

        }

        //Tested
        public bool IsAbsoluteUriLinkStoredInFailedResults(SortedDictionary<string, CrawledLinkInfo> crawlFailedResults, CrawledLinkInfo crawledLinkInfoFound)
        {
            _semaphore3.Wait();
            try
            {
                //Case: The link as already been stored in the Failed Results
                if ((crawledLinkInfoFound.AbsoluteLink !=null) && crawlFailedResults.ContainsKey(crawledLinkInfoFound?.AbsoluteLink.ToString()))
                {
                    return true;
                }
                return false;
            }
            finally { _semaphore3.Release(); }
        }

        //Tested
        public bool IsAbsoluteUriLinkJobEnqueued(Queue<CrawlJob> jobQueue, CrawledLinkInfo crawledLinkInfoFound)
        {
            _semaphore3.Wait();
            try
            {
                List<CrawlJob> urls = jobQueue.Where(a => a.Url.ToString().ToLower() == crawledLinkInfoFound?.AbsoluteLink?.ToString().ToLower()).ToList();
                return (urls.Count > 0) ? true : false;
            }
            finally { _semaphore3.Release(); }
        }
    }
}
