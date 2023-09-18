using ServiceLayer;
using ServiceLayer.Interfaces;
using ServiceLayer.Messages;
using ServiceLayer.Models;
using System.Windows.Controls;

namespace UnitTestTestWebCrawlerScraper.ServiceLayerTests
{
    public class InputValidatorTest
    {
        IInputValidator _inputValidator;

        public InputValidatorTest()
        {
            ILinkInspector _linkInspector = new LinkInspector();
            _inputValidator = new InputValidator(_linkInspector);
        }


        [Fact]
        public void ValidateInputs_Correctnputs_ReturnsOK()
        {
            //Arrange

            bool searchAllPagesIsChecked = true;


            bool searchCountIsChecked = false;

            bool newRadioButtonIsChecked = true;
            InputsForValidation inputsForValidationOk = new InputsForValidation()
            {
                UrlText = "https://www.cnn.com",
                TotalPagesForSearchText = "3",
                RadioBtnSearchAllPagesChecked = searchAllPagesIsChecked,
                RadioBtnSearchCountChecked = searchCountIsChecked,
                ReportFolder = @"c:\test"
            };

            //Act
            var actualResult = _inputValidator.ValidateCrawlInputs(inputsForValidationOk);
            //Assert
            Assert.True(actualResult.AllCrawlInputsAreValid);
            Assert.Equal(string.Empty, actualResult.UrlLabelReport);
            Assert.Equal(string.Empty, actualResult.PagesCountLabelReport);
            Assert.Equal(string.Empty, actualResult.OptionsLabelReport);
            Assert.Equal(string.Empty, actualResult.SelectedFolderReport);
        }


        [Fact]
        public void ValidateInputs_NullInputs_ReturnsFail()
        {
            //Arrange
           
            bool searchAllPagesIsChecked = true;
           
            bool searchCountIsChecked = false;

             bool newRadioButtonIsChecked = true;
            InputsForValidation inputsForValidationOk = new InputsForValidation()
            {
                UrlText = null,
                TotalPagesForSearchText = null,
                RadioBtnSearchAllPagesChecked = searchAllPagesIsChecked,
                RadioBtnSearchCountChecked = searchCountIsChecked,
                ReportFolder = null
            };

            //Act
            var actualResult = _inputValidator.ValidateCrawlInputs(inputsForValidationOk);
            //Assert
            Assert.True(!actualResult.AllCrawlInputsAreValid);
            Assert.Equal(NotificationMessage.WarningUrlMalformed, actualResult.UrlLabelReport);
            Assert.Equal(NotificationMessage.WarningSelectNumberGreaterThanZero, actualResult.PagesCountLabelReport);
            Assert.Equal(string.Empty, actualResult.OptionsLabelReport);
            Assert.Equal(NotificationMessage.WarningSelectFolder, actualResult.SelectedFolderReport);
        }

        [Fact]
        public void ValidateInputs_MixedInputs_ReturnsFail()
        {
            //Arrange

            bool searchAllPagesIsChecked = true;

            bool searchCountIsChecked = false;

            bool newRadioButtonIsChecked = true;
            InputsForValidation inputsForValidationOk = new InputsForValidation()
            {
                UrlText = string.Empty,
                TotalPagesForSearchText = string.Empty,
                RadioBtnSearchAllPagesChecked = searchAllPagesIsChecked,
                RadioBtnSearchCountChecked = searchCountIsChecked,
                ReportFolder = string.Empty
            };

            //Act
            var actualResult = _inputValidator.ValidateCrawlInputs(inputsForValidationOk);
            //Assert
            Assert.True(!actualResult.AllCrawlInputsAreValid);
            Assert.Equal(NotificationMessage.WarningUrlMalformed, actualResult.UrlLabelReport);
            Assert.Equal(NotificationMessage.WarningSelectNumberGreaterThanZero, actualResult.PagesCountLabelReport);
            Assert.Equal(string.Empty, actualResult.OptionsLabelReport);
            Assert.Equal(NotificationMessage.WarningSelectFolder, actualResult.SelectedFolderReport);
        }

        [Fact]
        public void ValidateScrapeInputs_CorrectInputs_ReturnsOk()
        {
            //Arrange
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("test");
            InputsForValidation scrapeInputsForValidation = new InputsForValidation()
            {
                ScrapingFolderPath = @"c:\text\test2",
                AllCheckedTxtFiles = queue,
                XPathExpression = "//h2"
            };

            //Act
            var actualResult = _inputValidator.ValidateScrapeInputs(scrapeInputsForValidation);
            //Assert
            Assert.True(actualResult.AllScrapeInputsAreValid);
            Assert.Equal(string.Empty, actualResult.ScrapingFolderPathReport);
            Assert.Equal(string.Empty, actualResult.AlltxtFilesReport);
            Assert.Equal(string.Empty, actualResult.XPathExpressionReport);
        }

        [Fact]
        public void ValidateScrapeInputs_NullInputs_ReturnsFail()
        {
            //Arrange           
            InputsForValidation scrapeInputsForValidation = new InputsForValidation()
            {
                ScrapingFolderPath = null,
                AllCheckedTxtFiles = null,
                XPathExpression = null
            };

            //Act
            var actualResult = _inputValidator.ValidateScrapeInputs(scrapeInputsForValidation);
            //Assert
            Assert.True(!actualResult.AllScrapeInputsAreValid);
            Assert.Equal(NotificationMessage.WarningScrapingFolder, actualResult.ScrapingFolderPathReport);
            Assert.Equal(NotificationMessage.WarningTextFileNotSelected, actualResult.AlltxtFilesReport);
            Assert.Equal(NotificationMessage.WarningXPathExpression, actualResult.XPathExpressionReport);
        }

        [Fact]
        public void ValidateScrapeInputs_MixedInputs_ReturnsFail()
        {
            //Arrange           
            InputsForValidation scrapeInputsForValidation = new InputsForValidation()
            {
                ScrapingFolderPath = string.Empty,
                AllCheckedTxtFiles = new Queue<string>(),
                XPathExpression = string.Empty
            };

            //Act
            var actualResult = _inputValidator.ValidateScrapeInputs(scrapeInputsForValidation);
            //Assert
            Assert.True(!actualResult.AllScrapeInputsAreValid);
            Assert.Equal(NotificationMessage.WarningScrapingFolder, actualResult.ScrapingFolderPathReport);
            Assert.Equal(NotificationMessage.WarningTextFileNotSelected, actualResult.AlltxtFilesReport);
            Assert.Equal(NotificationMessage.WarningXPathExpression, actualResult.XPathExpressionReport);
        }
    }
}
