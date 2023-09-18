using DataAccessLayer.IOFiles.Models;
using ServiceLayer.Enumerations;

namespace DataAccessLayer.Utils.Interfaces
{
    public interface IFileHelper
    {
        string CreateUniqueName(string originalName, FileExtension? fileExtension);
        string RemoveSpecialCharacters(string originaFileName);
        string ConvertCsvDataPropertiesTostring(CsvDataFormat data);
    }
}
