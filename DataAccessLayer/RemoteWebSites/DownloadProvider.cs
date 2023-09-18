using OpenQA.Selenium.Chrome;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.RemoteWebSites
{
    public class DownloadProvider : Utils.Interfaces.IDownloadProvider
    {
        private static readonly SemaphoreSlim _semaphore2 = new SemaphoreSlim(1, 1);
        private IPage? _puppeteerPage;
        private ChromeDriver? _seleniumChromeDriverPage;

        public DownloadProvider()
        {
            _puppeteerPage = null;
            _seleniumChromeDriverPage = null;
        }

        public async Task<string> DownloadPageAsyncUsingPuppeteer(string fullUrl)
        {
            await _semaphore2.WaitAsync();
            try
            {
                IPage page = await CreatePuppeteerInstance();
                await page.GoToAsync(fullUrl);
                string content = await page.GetContentAsync();

                return content;
            }
            catch (Exception ex)
            {
                var info = ex.Message;
                return null;
            }
            finally
            {
                _semaphore2.Release();
            }
        }

        public string DownloadPageUsingSelenium(string fullUrl)
        {
            _semaphore2.Wait();
            try
            {
                ChromeDriver browser = CreateSelemiumChromeDriverInstance();
                browser.Navigate().GoToUrl(fullUrl);
                var content = browser.PageSource;
                return content;
            }
            catch (Exception ex)
            {
                var info = ex.Message;
                return null;
            }
            finally
            {
                _semaphore2.Release();
            }
        }

        #region Private Methods
        private ChromeDriver CreateSelemiumChromeDriverInstance()
        {
            if (_seleniumChromeDriverPage == null)
            {
                _seleniumChromeDriverPage = GetSeleniumInstance();
            }
            return _seleniumChromeDriverPage;
        }

        private ChromeDriver GetSeleniumInstance()
        {

            try
            {
                ChromeOptions options = new ChromeOptions()
                {
                    BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
                };
                options.AddArguments(new List<string>() { "headless", "disable-gpu" });


                ChromeDriver browser = new ChromeDriver(options);
                return browser;
            }
            catch (Exception ex)
            {
                var info = ex.Message;
            }
            return null;
        }

        private async Task<IPage> CreatePuppeteerInstance()
        {
            if (_puppeteerPage == null)
            {
                _puppeteerPage = await GetPuppeteerInstanceAsync();
            }
            return _puppeteerPage;
        }


        private async Task<IPage> GetPuppeteerInstanceAsync()
        {
            try
            {
                using (BrowserFetcher browserFetcher = new BrowserFetcher())
                {
                    //await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
                    await browserFetcher.DownloadAsync();
                    LaunchOptions launchOptions = new LaunchOptions() { Headless = true };
                    IBrowser browser = await Puppeteer.LaunchAsync(launchOptions);

                    IPage page = await browser.NewPageAsync();

                    return page;
                }
            }
            catch (Exception ex)
            {
                var info = ex.Message;
            }
            return null;
        }

        #endregion Private Methods
    }
}
