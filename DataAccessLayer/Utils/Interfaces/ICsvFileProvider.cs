namespace DataAccessLayer.Utils.Interfaces
{
    public interface ICsvFileProvider
    {
        bool CreateCsvFile(string fileFullPathName);

        bool CreateHeaderCsvFile(string fileFullPathName);

        bool AddSingleRecordToCsvFile(string fileFullPathName, string record);
    }
}
