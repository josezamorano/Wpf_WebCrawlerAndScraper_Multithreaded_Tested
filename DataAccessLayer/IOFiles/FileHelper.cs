using DataAccessLayer.IOFiles.Models;
using DataAccessLayer.Utils.Interfaces;
using ServiceLayer.Enumerations;
using System;
using System.Text;
using System.Threading;

namespace DataAccessLayer.IOFiles
{
    public class FileHelper : IFileHelper
    {
        Random _random = new Random();
        private static readonly SemaphoreSlim _semaphore8 = new SemaphoreSlim(1, 1);

        //Tested
        public string CreateUniqueName(string originalName, FileExtension? fileExtension)
        {
            _semaphore8.Wait();
            try
            {
                string? sanitizedFileExtension = (fileExtension == null) ? string.Empty : Enum.GetName(typeof(FileExtension), fileExtension);
                string sanitizeName = RemoveSpecialCharacters(originalName);
                var value = _random.Next(10000000, 99999999);
                string year = DateTime.Now.Year.ToString();
                string month = (DateTime.Now.Month < 10) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
                string day = (DateTime.Now.Day < 10) ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
                string Hour = DateTime.Now.Hour.ToString();
                string Minutes = DateTime.Now.Minute.ToString();
                string seconds = DateTime.Now.Second.ToString();

                string dotValue = (sanitizedFileExtension == string.Empty) ? string.Empty : ".";

                string uniqueFileName = sanitizeName + value + "_" + year + month + day + "_" + Hour + Minutes + seconds + dotValue + sanitizedFileExtension;

                return uniqueFileName;
            }
            finally { _semaphore8.Release(); }
        }

        //Tested
        public string RemoveSpecialCharacters(string originaFileName)
        {
            StringBuilder sb = new StringBuilder(originaFileName);
            sb.Replace("/", "");
            sb.Replace(":", "");
            sb.Replace("=", "");
            sb.Replace("?", "");
            sb.Replace("&", "and");
            sb.Replace("#", "");
            sb.Replace("@", "");
            sb.Replace("+", "");
            sb.Replace("(", "");
            sb.Replace(")", "");
            sb.Replace(",", "");
            sb.Replace("  ", "");
            sb.Replace(" ", "-");
            sb.Replace("'", "");
            sb.Replace("*", "");
            sb.Replace(".", "");
            sb.Replace("eacute;", "é");

            string cleanFileName = sb.ToString().ToLower();

            return cleanFileName;
        }

        //Tested
        public string ConvertCsvDataPropertiesTostring(CsvDataFormat data)
        {
            _semaphore8.Wait();
            try
            {
                string? linkStatus = (data.LinkStatus == null) ? null : Enum.GetName(typeof(LinkStatus), data.LinkStatus);
                string? webSitePageStatus = (data.WebSitePageStatus == null) ? null : Enum.GetName(typeof(WebSitePageStatus), data.WebSitePageStatus);
                StringBuilder sb = new StringBuilder();
                sb.Append(data.FullyQualifiedUrl);
                sb.Append(",");
                sb.Append(data.AbsoluteLink?.ToString());
                sb.Append(",");
                sb.Append(data.AbsoluteLinkStringFormatted);
                sb.Append(",");
                sb.Append(linkStatus);
                sb.Append(",");
                sb.Append(data.IsLinkFullyQualified.ToString());
                sb.Append(",");
                sb.Append(data.Level);
                sb.Append(",");
                sb.Append(data.OriginalLink);
                sb.Append(",");
                sb.Append(data.ParentPageUrl);
                sb.Append(",");
                sb.Append(data.SavedFullFileName);
                sb.Append(",");
                sb.Append(webSitePageStatus);

                string result = sb.ToString();

                return result;
            }

            finally { _semaphore8.Release(); }
        }
    }
}
