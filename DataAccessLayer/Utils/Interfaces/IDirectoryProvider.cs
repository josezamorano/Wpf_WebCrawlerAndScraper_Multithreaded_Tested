namespace DataAccessLayer.Utils.Interfaces
{
    public interface IDirectoryProvider
    {
        bool CreateDirectoryIfNotExist(string directoryFullPath);

        string[] DirectoryGetFiles(string filePath);

        string[] DirectoryGetDirectories(string folderFullPath);
    }
}
