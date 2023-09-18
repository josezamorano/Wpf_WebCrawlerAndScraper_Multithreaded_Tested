using DataAccessLayer.IOFiles.Models;
using ServiceLayer.Enumerations;
using ServiceLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Utils.Interfaces
{
    public interface IFileActivityManager
    {
        string CreateFullPathFileName(string originalFileName, string folderFullPath, FileExtension? fileExtension);

        CsvDataFormat CreateSingleCsvRecord(KeyValuePair<string, CrawledLinkInfo> crawledLink);

        bool CreateCsvFile(string fileFullPathName);

        bool CreateHeaderInCsvFile(string fileFullPathName);

        bool SaveSingleRecordToCSVFile(string fileFullPathName, CsvDataFormat record);

        bool SaveSingleRecordToCSVFile(string fileFullPathName, string record);

        bool WriteToTxtFile(string fullFilePath, string fileContent);

        string ReadFromTxtFile(string fullFilePath);

        List<string> GetAllFilesInDirectory(string folderFullPath);

        bool CreateDirectoryIfNotExist(string directoryFullPath);

        string GetFileName(string fileFullPath);
    }
}
