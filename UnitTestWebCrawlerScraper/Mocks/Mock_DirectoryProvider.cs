using DataAccessLayer.Utils.Interfaces;

namespace UnitTestTestWebCrawlerScraper.Mocks
{
    public class Mock_DirectoryProvider : IDirectoryProvider
    {
        public bool CreateDirectoryIfNotExist(string directoryFullPath)
        {
            return true;
        }

        public string[] DirectoryGetDirectories(string folderFullPath)
        {
            return new string[0];
        }

        public string[] DirectoryGetFiles(string filePath)
        {
            string file1 = @"c:\test\test1.txt";
            string file2 = @"c:\test\test2.txt";
            string file3 = @"c:\test\test3.txt";

            return new string[3] { file1, file2, file3 };
        }
    }
}
