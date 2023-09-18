using DataAccessLayer.Utils.Interfaces;

namespace UnitTestTestWebCrawlerScraper.Mocks
{
    public class Mock_DownloadProvider : IDownloadProvider
    {
        public Task<string> DownloadPageAsyncUsingPuppeteer(string fullUrl)
        {
            throw new NotImplementedException();
        }

        public string DownloadPageUsingSelenium(string fullUrl)
        {
            string htmlContent = @"<html> 
                                  <a href=""https://www.cnn.com""> Go to Here </a> 
                                  <a href=""mailto:contact@html.com""> get in touch</a> 
                                  <a href=""#Specify_a_Hyperlink_Target_href"">This first anchor element</a>
                                 </html>";

            return htmlContent;
        }
    }
}
