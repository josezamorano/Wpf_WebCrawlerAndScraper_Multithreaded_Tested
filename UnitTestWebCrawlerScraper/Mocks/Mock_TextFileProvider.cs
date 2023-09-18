using DataAccessLayer.Utils.Interfaces;

namespace UnitTestTestWebCrawlerScraper.Mocks
{
    public class Mock_TextFileProvider : ITextFileProvider
    {
        public string ReadFromTextFile(string fullFilePath)
        {
            return "this is a test";
        }

        public bool WriteToTextFile(string fullFilePath, string htmlFileContent)
        {
            return true;
        }
    }
}
