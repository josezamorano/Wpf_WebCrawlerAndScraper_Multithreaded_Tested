using DomainLayer.Utils.Interfaces;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DomainLayer
{
    public class HtmlParser : IHtmlParser
    {
        private static readonly SemaphoreSlim _semaphore4 = new SemaphoreSlim(1, 1);


        //Tested
        public List<string> GetLinks(string htmlContent)
        {
            _semaphore4.Wait();
            try
            {
                List<string> links = new List<string>();
                var filteredLinks = (string.IsNullOrWhiteSpace(htmlContent)) ? links : FilterLinks(htmlContent);
                return filteredLinks;
            }
            finally { _semaphore4.Release(); }

        }

        #region Private Methods
        private List<string> FilterLinks(string htmlContent)
        {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlContent);
            HtmlNodeCollection linkNodes = document.DocumentNode.SelectNodes("//a[@href]");
            List<string> filteredNodes = (linkNodes == null) ? new List<string>() : GetFilteredLinks(linkNodes);

            return filteredNodes;
        }

        private List<string> GetFilteredLinks(HtmlNodeCollection linkNodes)
        {
            List<HtmlAttribute> hrefAttributes = linkNodes.Where(link => link.Attributes.Contains("href")).Select(node => node.Attributes["href"]).ToList();
            List<string> links = hrefAttributes.Select(attribute => attribute.Value).ToList();

            return links;
        }

        #endregion Private Methods
    }
}
