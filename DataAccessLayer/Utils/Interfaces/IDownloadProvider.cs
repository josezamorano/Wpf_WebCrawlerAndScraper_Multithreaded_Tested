using System.Threading.Tasks;

namespace DataAccessLayer.Utils.Interfaces
{
    public interface IDownloadProvider
    {
        Task<string> DownloadPageAsyncUsingPuppeteer(string fullUrl);
        string DownloadPageUsingSelenium(string fullUrl);
    }
}
