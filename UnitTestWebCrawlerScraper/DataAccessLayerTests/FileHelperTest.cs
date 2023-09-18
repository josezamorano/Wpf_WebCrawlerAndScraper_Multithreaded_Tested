using DataAccessLayer.IOFiles;
using DataAccessLayer.IOFiles.Models;
using DataAccessLayer.Utils.Interfaces;
using ServiceLayer.Enumerations;

namespace UnitTestTestWebCrawlerScraper.DataAccessLayerTests
{
    public class FileHelperTest
    {
        IFileHelper _fileHelper;
        public FileHelperTest()
        {
            _fileHelper = new FileHelper();
        }

        [Fact]
        public void CreateUniqueFileName_InputSameDataReturnsDifferentName()
        {
            //Arrange
            string initialFileName = "testFile";
            //Act
            var actualResult1 = _fileHelper.CreateUniqueName(initialFileName, FileExtension.txt);
            var actualResult2 = _fileHelper.CreateUniqueName(initialFileName, FileExtension.txt);

            //Assert
            Assert.NotEqual(actualResult1, actualResult2);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateUniqueFileName_InitialFileNameIsEmpty_ReturnsOk(string originalName)
        {
            //Act
            var actualResult = _fileHelper.CreateUniqueName(originalName, FileExtension.txt);
            //Assert
            Assert.NotEmpty(actualResult);
        }

        [Theory]
        [InlineData("https://eyehunt.com/dlink.do?userName=peter@nable.com&password=123", "=")]
        [InlineData("https://eyehunt.com/dlink.do?userName=peter@nable.com&password=123", "/")]
        [InlineData("https://eyehunt.com/dlink.do?userName=peter@nable.com&password=123", ".")]
        [InlineData("https://eyehunt.com/dlink.do?userName=peter@nable.com&password=123", "@")]
        [InlineData("https://eyehunt.com/dlink.do?userName=peter@nable.com&password=123", ":")]
        [InlineData("https://eyehunt.com/dlink.do?userName=peter@nable.com&password=123", "?")]
        [InlineData("http://example.com/index.html?url=+encodeURIComponent(myUrl)", "+")]
        [InlineData("http://example.com/index.html?url=+encodeURIComponent(myUrl)", "(")]
        [InlineData("http://example.com/index.html?url=+encodeURIComponent(myUrl)", ")")]
        public void RemoveSpecialCharacters_InputUrlSpecialChars_RemovesAllOk(string originalFileName, string expectedSubstring)
        {
            //Act
            var actualResult = _fileHelper.RemoveSpecialCharacters(originalFileName);
            //Assert
            Assert.DoesNotContain(expectedSubstring, actualResult);
        }

        [Fact]
        public void ConvertCsvDataPropertiesTostring_ValidInputs_ReturnsOk()
        {
            //Arrange
            CsvDataFormat csvDataFormat = new CsvDataFormat()
            {
                FullyQualifiedUrl = "test",
                AbsoluteLink = new Uri("http://this_is_a_test.com"),
                AbsoluteLinkStringFormatted = "this_is_a_test",
                LinkStatus = LinkStatus.Found_ForValidation,
                IsLinkFullyQualified = false,
                Level = 2,
                OriginalLink = "http://test.com",
                ParentPageUrl = "http://testParent.com",
                SavedFullFileName = "test",
                WebSitePageStatus = WebSitePageStatus.NotDownloaded
            };
            //Act
            string actualResult = _fileHelper.ConvertCsvDataPropertiesTostring(csvDataFormat);
            //Assert
            Assert.IsType<string>(actualResult);
            Assert.Contains("2", actualResult);
        }

        [Fact]
        public void ConvertCsvDataPropertiesTostring_NullInputs_ReturnsOk()
        {
            //Arrange
            var obj1 = new CsvDataFormat()
            {
                FullyQualifiedUrl = null,
                AbsoluteLink = null,
                AbsoluteLinkStringFormatted = null,
                LinkStatus = null,
                IsLinkFullyQualified = null,
                Level = null,
                OriginalLink = null,
                ParentPageUrl = null,
                SavedFullFileName = null,
                WebSitePageStatus = null
            };
            //Act
            string actualResult = _fileHelper.ConvertCsvDataPropertiesTostring(obj1);
            //Assert
            Assert.IsType<string>(actualResult);
            Assert.Contains(",", actualResult);
        }
    }
}
