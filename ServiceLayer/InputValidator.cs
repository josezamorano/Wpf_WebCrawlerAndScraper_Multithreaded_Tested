using ServiceLayer.Interfaces;
using ServiceLayer.Messages;
using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ServiceLayer
{
    public class InputValidator : IInputValidator
    {

        CrawlInputValidationReport _crawlInputValidationReport;
        ScrapeInputValidationReport _scrapeInputValidationReport;

        ILinkInspector _linkInspector;
        public InputValidator(ILinkInspector linkInspector)
        {
            _linkInspector = linkInspector;
            _crawlInputValidationReport = new CrawlInputValidationReport();
            _scrapeInputValidationReport = new ScrapeInputValidationReport();
        }

        //Tested
        public CrawlInputValidationReport ValidateCrawlInputs(InputsForValidation inputsForValidation)
        {
            SetCrawlReportInitialState();

            bool _urlIsValid = ResolveUriValidation(inputsForValidation.UrlText);
            bool _optionsAreValid = ResolveSearchOptionsValidation(inputsForValidation.RadioBtnSearchAllPagesChecked, inputsForValidation.RadioBtnSearchCountChecked);
            bool _totalPagesForSearchAreValid = ResolveTotalPagesForSearchValidation(inputsForValidation.TotalPagesForSearchText, inputsForValidation.RadioBtnSearchCountChecked);
            bool _folderIsSelected = ResolveReportFolderValidation(inputsForValidation.ReportFolder);

            if (!_urlIsValid || !_optionsAreValid || !_totalPagesForSearchAreValid || !_folderIsSelected)
            {
                _crawlInputValidationReport.AllCrawlInputsAreValid = false;
            }

            return _crawlInputValidationReport;
        }

        //Tested
        public ScrapeInputValidationReport ValidateScrapeInputs(InputsForValidation inputsForValidation)
        {
            SetScrapeReportInitialState();

            bool scrapingFolderIsValid = ResolveCrapingFolderPath(inputsForValidation.ScrapingFolderPath);
            bool checkedFoldersAreValid = ResolveCheckedTextFiles(inputsForValidation.AllCheckedTxtFiles);
            bool xPathExpressionIsValid = ResolveXPathExpression(inputsForValidation.XPathExpression);

            if (!scrapingFolderIsValid || !checkedFoldersAreValid || !xPathExpressionIsValid)
            {
                _scrapeInputValidationReport.AllScrapeInputsAreValid = false;
            }

            return _scrapeInputValidationReport;
        }


        #region Private Methods 

        private void SetCrawlReportInitialState()
        {
            _crawlInputValidationReport.AllCrawlInputsAreValid = true;
            _crawlInputValidationReport.UrlLabelReport = string.Empty;
            _crawlInputValidationReport.OptionsLabelReport = string.Empty;
            _crawlInputValidationReport.PagesCountLabelReport = string.Empty;
            _crawlInputValidationReport.SelectedFolderReport = string.Empty;
        }

        private bool ResolveUriValidation(string urlText)
        {
            Uri urlForSearch;
            bool uriIsValid = Uri.TryCreate(urlText, UriKind.Absolute, out urlForSearch);

            var uriIsWellFormatted = (uriIsValid) ? (_linkInspector.IsUriSchemeValid(urlForSearch.Scheme)) : false;
            if (!uriIsWellFormatted)
            {
                _crawlInputValidationReport.UrlLabelReport = NotificationMessage.WarningUrlMalformed;
                return false;
            }

            return true;
        }

        private bool ResolveSearchOptionsValidation(bool radioBtnSearchAllPagesChecked, bool radioBtnSearchCountChecked)
        {
            
            if (radioBtnSearchAllPagesChecked != true && radioBtnSearchCountChecked != true)
            {
                _crawlInputValidationReport.OptionsLabelReport = NotificationMessage.WarningSelectAnOption;

                return false;
            }

            return true;
        }

        private bool ResolveTotalPagesForSearchValidation(string totalPagesForSearchText, bool radioBtnSearchCountChecked)
        {
            //Note: Validation just in case the variable totalPagesForSearchText ever comes without value due to some code change
            if (InputIsNullEmptyOrWhiteSpace(totalPagesForSearchText))
            {
                _crawlInputValidationReport.PagesCountLabelReport = NotificationMessage.WarningSelectNumberGreaterThanZero;

                return false;
            }

            int defaultValue = 0;
            var numberIsValid = Int32.TryParse(totalPagesForSearchText, out defaultValue);
            if (numberIsValid)
            {
                if ((radioBtnSearchCountChecked == true) && defaultValue < 1)
                {
                    _crawlInputValidationReport.PagesCountLabelReport = NotificationMessage.WarningSelectNumberGreaterThanZero;

                    return false;
                }
            }

            return true;
        }

        private bool ResolveReportFolderValidation(string reportFolder)
        {
            if (InputIsNullEmptyOrWhiteSpace(reportFolder))
            {
                _crawlInputValidationReport.SelectedFolderReport = NotificationMessage.WarningSelectFolder;

                return false;
            }

            return true;
        }

        private bool InputIsNullEmptyOrWhiteSpace(string input)
        {
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            return true;
        }

        private void SetScrapeReportInitialState()
        {
            _scrapeInputValidationReport.AllScrapeInputsAreValid = true;
            _scrapeInputValidationReport.AlltxtFilesReport = string.Empty;
            _scrapeInputValidationReport.ScrapingFolderPathReport = string.Empty;
            _scrapeInputValidationReport.XPathExpressionReport = string.Empty;
        }

        private bool ResolveCrapingFolderPath(string scrapeFolderPath)
        {
            if (InputIsNullEmptyOrWhiteSpace(scrapeFolderPath))
            {
                _scrapeInputValidationReport.ScrapingFolderPathReport = NotificationMessage.WarningScrapingFolder;

                return false;
            }
            return true;
        }

        private bool ResolveCheckedTextFiles(Queue<string> allCheckedTxtFiles)
        {
            if (allCheckedTxtFiles != null && allCheckedTxtFiles.Count > 0)
            {
                return true;
            }
            _scrapeInputValidationReport.AlltxtFilesReport = NotificationMessage.WarningTextFileNotSelected;

            return false;
        }

        private bool ResolveXPathExpression(string xPathExpression)
        {

            if (InputIsNullEmptyOrWhiteSpace(xPathExpression))
            {
                _scrapeInputValidationReport.XPathExpressionReport = NotificationMessage.WarningXPathExpression;

                return false;
            }
            return true;
        }

        #endregion Private Methods
    }
}
