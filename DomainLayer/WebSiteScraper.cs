using DataAccessLayer.Utils.Interfaces;
using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using HtmlAgilityPack;
using ServiceLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DomainLayer
{
    public class WebSiteScraper : IWebSiteScraper
    {
        private static readonly SemaphoreSlim semaphore11 = new SemaphoreSlim(1, 1);
        private int _totalPagesToScrape = 0;
        private int _pagesScraped = 0;
        IFileActivityManager _fileActivityManager;
        public WebSiteScraper(IFileActivityManager fileActivityManager)
        {
            _fileActivityManager = fileActivityManager;
        }

        //Tested
        public void RunScraperMultithreaded(WebScraperInfo webScraperInfo)
        {
            _pagesScraped = 0;
            Thread mainWorkerScraperThread = new Thread(() =>
            {
                _totalPagesToScrape = webScraperInfo.AllTextFilesToScrape.Count;
                string updatedFolder = CreateFolderNameForScrapedFiles(webScraperInfo.ScrapingFolderFullPath);
                string folderNameForScrapedFiles = updatedFolder.Replace(@"\\?\", "");
                var success = _fileActivityManager.CreateDirectoryIfNotExist(folderNameForScrapedFiles);

                List<Thread> allThreads = new List<Thread>();
                while (_pagesScraped < _totalPagesToScrape)
                {
                    if (webScraperInfo.AllTextFilesToScrape.Count == 0) { continue; }
                    Thread fileScraperThread = new Thread(() =>
                    {
                        string filePath = GetFilePath(webScraperInfo.AllTextFilesToScrape);
                        if (filePath == null) { return; }
                        string fileName = _fileActivityManager.GetFileName(filePath);
                        string fileNameScraped = _fileActivityManager.CreateFullPathFileName(fileName, folderNameForScrapedFiles, FileExtension.csv);
                        bool csvFileCreated = _fileActivityManager.CreateCsvFile(fileNameScraped);

                        var content = _fileActivityManager.ReadFromTxtFile(filePath);

                        HtmlAgilityPack.HtmlDocument htmlDoument = new HtmlAgilityPack.HtmlDocument();
                        htmlDoument.LoadHtml(content);

                        HtmlNodeCollection allHtmlNodes = htmlDoument.DocumentNode.SelectNodes(webScraperInfo.XPathExpression);
                        int countRecordsSaved = 0;
                        foreach (HtmlNode node in allHtmlNodes)
                        {
                            var resultSavedRecord = _fileActivityManager.SaveSingleRecordToCSVFile(fileNameScraped, node.InnerText);
                            countRecordsSaved++;
                        }
                        if (csvFileCreated && countRecordsSaved == allHtmlNodes.Count)
                        {
                            UpdateCountOfPagesScraped();
                        }
                    });
                    fileScraperThread.Name = "scraper_worker" + allThreads.Count;
                    fileScraperThread.IsBackground = true;
                    fileScraperThread.Start();
                    allThreads.Add(fileScraperThread);
                }
                AbortAllAliveThreads(allThreads);
                string report = $"{_pagesScraped} Scraped files were created in the Folder {folderNameForScrapedFiles}";
                webScraperInfo.ScrapeReportCallback(report);
            });
            mainWorkerScraperThread.Name = "mainWorkerScraperThread";
            mainWorkerScraperThread.IsBackground = true;
            mainWorkerScraperThread.Start();
        }

        private string GetFilePath(Queue<string> allTextFilesToScrape)
        {
            semaphore11.Wait();
            try
            {
                string filePath = (allTextFilesToScrape.Count > 0) ? allTextFilesToScrape.Dequeue() : null;
                return filePath;
            }
            finally { semaphore11.Release(); }
        }

        private void AbortAllAliveThreads(List<Thread> allThreads)
        {
            bool areAlive = true;

            while (areAlive)
            {
                int counter = 0;
                try
                {
                    foreach (var thread in allThreads)
                    {
                        if (thread.IsAlive)
                        {
                            thread.Abort();
                        }
                        else
                        {
                            counter++;
                        }
                    }
                    if (counter >= allThreads.Count)
                    {
                        areAlive = false;
                    }
                }
                catch (Exception ex)
                {
                    counter++;
                }
            }
        }

        private void UpdateCountOfPagesScraped()
        {
            semaphore11.Wait();
            try
            {
                _pagesScraped++;
            }
            finally { semaphore11.Release(); }

        }

        private string CreateFolderNameForScrapedFiles(string originalFolderFullPath)
        {
            var folderSections = originalFolderFullPath.Split('\\');
            string lastFolderSection = string.Empty;
            string folderPath = string.Empty;
            if (folderSections.Length > 0)
            {
                folderPath = createFilePath(folderSections);
                lastFolderSection = folderSections[folderSections.Length - 1];
                lastFolderSection += "_scraped";
            }
            var resultsFolder = _fileActivityManager.CreateFullPathFileName(lastFolderSection, folderPath, null);

            return resultsFolder;
        }

        private string createFilePath(string[] pathSections)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (var i = 0; i < pathSections.Length - 1; i++)
            {
                stringBuilder.Append(pathSections[i]);
                if (i < (pathSections.Length - 2))
                {
                    stringBuilder.Append("\\");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
