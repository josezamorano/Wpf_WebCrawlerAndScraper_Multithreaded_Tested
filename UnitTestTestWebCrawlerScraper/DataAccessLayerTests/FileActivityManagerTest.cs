using DataAccessLayer.IOFiles;
using DataAccessLayer.IOFiles.Models;
using DataAccessLayer.Utils.Interfaces;
using ServiceLayer.Enumerations;
using ServiceLayer.Models;
using UnitTestTestWebCrawlerScraper.Mocks;

namespace UnitTestTestWebCrawlerScraper.DataAccessLayerTests
{
    public class FileActivityManagerTest
    {
        IFileActivityManager _fileActivityManager;
        public FileActivityManagerTest()
        {
            IFileHelper _fileHelper = new FileHelper();
            IPathProvider _pathProvider = new PathProvider();
            ICsvFileProvider _csvFileProvider = new Mock_CsvFileProvider();
            ITextFileProvider _textFileProvider = new Mock_TextFileProvider();
            IDirectoryProvider _directoryProvider = new Mock_DirectoryProvider();
            _fileActivityManager = new FileActivityManager(_fileHelper, _pathProvider, _csvFileProvider, _textFileProvider, _directoryProvider);
        }

        [Fact]
        public void CreateFullPathFileName_ValidInputs_ReturnsOk()
        {
            //Arrange
            string folderFullPath = "c:\testFolder";
            string originalFileName = "test";
            FileExtension fileExtension = FileExtension.txt;
            string? extensionName = Enum.GetName(typeof(FileExtension), fileExtension);
            //Act
            var actualResult = _fileActivityManager.CreateFullPathFileName(originalFileName, folderFullPath, fileExtension);
            //Assert
            Assert.Contains(folderFullPath, actualResult);
            Assert.Contains(originalFileName, actualResult);

            Assert.Contains(extensionName, actualResult);

        }

        [Fact]
        public void CreateFullPathFileName_SpecialInputs_ReturnsOk()
        {
            //Arrange
            string folderFullPath = null;
            string originalFileName = null;
            FileExtension? fileExtension = null;
            //Act
            var actualResult = _fileActivityManager.CreateFullPathFileName(originalFileName, folderFullPath, fileExtension);

            //Assert
            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult);

        }

        [Fact]
        public void CreateCsvFile_ValidInputs_ReturnOK()
        {
            //Arrange
            string filePath = "";
            //Act
            var actualResult = _fileActivityManager.CreateCsvFile(filePath);
            //Assert
            Assert.True(actualResult);
        }


        [Fact]
        public void CreateSingleCsvRecord_ValidInputs_ReturnsOk()
        {
            //Arrange
            string url = "https://www.test.com";
            CrawledLinkInfo crawledLinkInfo = new CrawledLinkInfo()
            {
                AbsoluteLink = new Uri(url),
                AbsoluteLinkStringFormatted = url,
                LinkStatus = LinkStatus.Valid_InternalChildPage,
                IsLinkFullyQualified = true,
                Level = 5,
                OriginalLink = url,
                ParentPageUrl = "https://www.parent.com",
                WebSitePage = "page info",
                WebSitePageFullFileName = "c://test/page",
                WebSitePageStatus = WebSitePageStatus.NotDownloaded
            };

            KeyValuePair<string, CrawledLinkInfo> keyValuePair = new KeyValuePair<string, CrawledLinkInfo>(url, crawledLinkInfo);
            //Act
            CsvDataFormat result = _fileActivityManager.CreateSingleCsvRecord(keyValuePair);
            //Assert
            Assert.IsType<CsvDataFormat>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateSingleCsvRecord_NullInputs_ReturnsOk()
        {
            //Arrange
            string url = null;
            CrawledLinkInfo crawledLinkInfo = new CrawledLinkInfo()
            {
                AbsoluteLink = null,
                AbsoluteLinkStringFormatted = null,
                LinkStatus = null,
                IsLinkFullyQualified = false,
                Level = 0,
                OriginalLink = null,
                ParentPageUrl = null,
                WebSitePage = null,
                WebSitePageFullFileName = null,
                WebSitePageStatus = null
            };

            KeyValuePair<string, CrawledLinkInfo> keyValuePair = new KeyValuePair<string, CrawledLinkInfo>(url, crawledLinkInfo);
            //Act
            CsvDataFormat result = _fileActivityManager.CreateSingleCsvRecord(keyValuePair);
            //Assert
            Assert.IsType<CsvDataFormat>(result);
            Assert.Null(result.AbsoluteLink);
        }

        [Fact]
        public void CreateHeaderInCsvFile_ValidInputs_ReturnsOk()
        {
            //Arrange
            string fullFileName = "/test";
            //Act
            var actualResult = _fileActivityManager.CreateHeaderInCsvFile(fullFileName);
            //ASsert
            Assert.True(actualResult);
        }

        [Fact]
        public void SaveSingleRecordToCSVFile_CsvDataFormat_ValidInputs_ReturnOk()
        {
            //Arrange
            string fullFileName = "c:\test\file.txt";
            CsvDataFormat dataFormat = new CsvDataFormat();
            //Act
            var actualResult = _fileActivityManager.SaveSingleRecordToCSVFile(fullFileName, dataFormat);
            //Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void SaveSingleRecordToCSVFile_String_validInputs_ReturnOk()
        {
            //Arrange
            string fullFileName = string.Empty;
            string record = string.Empty;
            //Act
            var actualResult = _fileActivityManager.SaveSingleRecordToCSVFile(fullFileName, record);
            //Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void WriteToTxtFile_ValidInputs_ReturnOk()
        {
            //Arrange
            string filePath = @"c:\test\File\";
            string fileContent = "<html> this is content</html>";
            //Act
            var actualResult = _fileActivityManager.WriteToTxtFile(filePath, fileContent);
            //Assert
            Assert.True(actualResult);
        }

        [Fact]
        public void ReadFromTxtFile_ValidInputs_ReturnsOk()
        {
            //Arrange
            string filePath = string.Empty;
            //Act
            var actualResult = _fileActivityManager.ReadFromTxtFile(filePath);
            bool containsText = (actualResult.Length > 0);
            //Assert
            Assert.True(containsText);
        }

        [Fact]
        public void GetAllFilesInDirectory_ValidInputs_ReturnsOk()
        {
            //Arrange
            string folderFullPath = string.Empty;
            //Act
            var actualResult = _fileActivityManager.GetAllFilesInDirectory(folderFullPath);

            //Assert
            Assert.True(actualResult.Count > 0);
        }

        [Fact]
        public void CreateDirectoryIfNotExist_ValidInputs_ReturnsOk()
        {
            //Arrange
            var directory = string.Empty;
            //Act
            var actualResult = _fileActivityManager.CreateDirectoryIfNotExist(directory);
            //Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(@"c:\test\text1.txt", "text1.txt")]
        [InlineData(null, null)]
        [InlineData("", "")]
        public void GetFileName_VariousInputs(string fileName, string expectedResult)
        {
            //Act
            var actualResult = _fileActivityManager.GetFileName(fileName);
            //Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
