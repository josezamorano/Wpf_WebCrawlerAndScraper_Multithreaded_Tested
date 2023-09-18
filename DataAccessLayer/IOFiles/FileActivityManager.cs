using DataAccessLayer.IOFiles.Models;
using DataAccessLayer.Utils.Interfaces;
using ServiceLayer.Enumerations;
using ServiceLayer.Models;
using System.Collections.Generic;
using System.Threading;

namespace DataAccessLayer.IOFiles
{
    public class FileActivityManager : IFileActivityManager
    {
        IFileHelper _fileHelper;
        IPathProvider _pathProvider;
        ICsvFileProvider _csvFileProvider;
        ITextFileProvider _textFileProvider;
        IDirectoryProvider _directoryProvider;
        private static readonly SemaphoreSlim _semaphore5 = new SemaphoreSlim(1, 1);

        public FileActivityManager(IFileHelper fileHelper, IPathProvider pathProvider, ICsvFileProvider csvFileProvider, ITextFileProvider textFileProvider, IDirectoryProvider directoryProvider)
        {
            _fileHelper = fileHelper;
            _pathProvider = pathProvider;
            _csvFileProvider = csvFileProvider;
            _textFileProvider = textFileProvider;
            _directoryProvider = directoryProvider;
        }

        //Tested
        public string CreateFullPathFileName(string originalFileName, string folderFullPath, FileExtension? fileExtension)
        {
            _semaphore5.Wait();
            try
            {
                string uniqueFileName = _fileHelper.CreateUniqueName(originalFileName, fileExtension);
                string fullName = @"\\?\" + folderFullPath + @"\" + uniqueFileName;
                return fullName;
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public bool CreateCsvFile(string fileFullPathName)
        {
            _semaphore5.Wait();
            try
            {
                var created = _csvFileProvider.CreateCsvFile(fileFullPathName);
                return created;
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public CsvDataFormat CreateSingleCsvRecord(KeyValuePair<string, CrawledLinkInfo> crawledLink)
        {
            _semaphore5.Wait();
            try
            {
                var crawledLinkInfo = crawledLink.Value;
                CsvDataFormat record = new CsvDataFormat()
                {
                    FullyQualifiedUrl = crawledLink.Key,
                    AbsoluteLink = crawledLinkInfo.AbsoluteLink,
                    LinkStatus = crawledLinkInfo.LinkStatus,
                    IsLinkFullyQualified = crawledLinkInfo.IsLinkFullyQualified,
                    Level = crawledLinkInfo.Level,
                    OriginalLink = crawledLinkInfo.OriginalLink,
                    ParentPageUrl = crawledLinkInfo.ParentPageUrl,
                    SavedFullFileName = crawledLinkInfo.WebSitePageFullFileName,
                    WebSitePageStatus = crawledLinkInfo.WebSitePageStatus
                };

                return record;
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public bool CreateHeaderInCsvFile(string fileFullPathName)
        {
            _semaphore5.Wait();
            try
            {
                bool result = _csvFileProvider.CreateHeaderCsvFile(fileFullPathName);
                return result;
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public bool SaveSingleRecordToCSVFile(string fileFullPathName, CsvDataFormat csvDataFormatRecord)
        {
            _semaphore5.Wait();
            try
            {
                string record = _fileHelper.ConvertCsvDataPropertiesTostring(csvDataFormatRecord);
                bool result = _csvFileProvider.AddSingleRecordToCsvFile(fileFullPathName, record);
                return result;
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public bool SaveSingleRecordToCSVFile(string fileFullPathName, string record)
        {
            _semaphore5.Wait();
            try
            {
                bool result = _csvFileProvider.AddSingleRecordToCsvFile(fileFullPathName, record);
                return result;
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public bool WriteToTxtFile(string fullFilePath, string fileContent)
        {
            _semaphore5.Wait();
            try
            {
                return _textFileProvider.WriteToTextFile(fullFilePath, fileContent);
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public string ReadFromTxtFile(string fullFilePath)
        {
            _semaphore5.Wait();
            try
            {
                return _textFileProvider.ReadFromTextFile(fullFilePath);
            }
            finally { _semaphore5.Release(); }
        }

        //Tested
        public List<string> GetAllFilesInDirectory(string folderFullPath)
        {
            List<string> files = new List<string>();
            try
            {
                string[] allFiles = _directoryProvider.DirectoryGetFiles(folderFullPath);
                foreach (string file in allFiles)
                {
                    files.Add(file);
                }
                string[] allDirectories = _directoryProvider.DirectoryGetDirectories(folderFullPath);
                foreach (string d in allDirectories)
                {
                    files.AddRange(GetAllFilesInDirectory(d));
                }
            }
            catch (System.Exception excpt)
            {
                files.Add(excpt.Message);
            }

            return files;
        }


        //Tested
        public bool CreateDirectoryIfNotExist(string directoryFullPath)
        {
            _semaphore5.Wait();
            try
            {
                return _directoryProvider.CreateDirectoryIfNotExist(directoryFullPath);
            }
            finally { _semaphore5.Release(); }
        }


        //Tested
        public string GetFileName(string fileFullPath)
        {
            _semaphore5.Wait();
            try
            {
                return _pathProvider.GetFileNameFromPath(fileFullPath);
            }
            finally { _semaphore5.Release(); }
        }
    }
}
