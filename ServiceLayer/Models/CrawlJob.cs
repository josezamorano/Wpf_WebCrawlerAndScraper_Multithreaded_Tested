using System;

namespace ServiceLayer.Models
{
    public class CrawlJob
    {
        public int PageNumber { get; set; }

        //Uniform Resource Identifier provides a technique for defining the identity of an item.
        public Uri Uri { get; set; }
        //Uniform Resource Locator is an identifier that only indicates the location of a web page. Full remote location path
        public string Url { get; set; }

        public CrawlJob(string url, int pageNumber)
        {
            PageNumber = pageNumber;
            Url = url;
            Uri = new Uri(url);
        }
    }
}
