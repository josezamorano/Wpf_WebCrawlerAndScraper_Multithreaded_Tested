using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using PresentationLayer.MVVM.Base;
using PresentationLayer.Utils.Interfaces;
using ServiceLayer.Constants;
using ServiceLayer.Interfaces;
using ServiceLayer.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static ServiceLayer.DelegateTypes.CustomDelegate;

namespace PresentationLayer.MVVM.ViewModels
{
    public class CrawlerViewModel : NotifyBaseViewModel, ICrawlerViewModel 
    {
        IDataCollectionManager _dataCollectionManager;
        IInputValidator _inputValidator;

        //Private Attributes
        private string _crawlerViewVisibility;
        private string _textBoxUrlForCrawler;
        private bool _textBoxTotalPagesIsReadOnly;

        private string _textBlockUrlWarning;
        private string _textBlockPagesSelectedWarning;
        private string _textBlockSearchTypeWarning;
        private string _textBlockFolderSelectorWarning;

        private string _textBoxTotalPages;
        private string _textBoxSelectedFolderPath;

        private bool _textBoxUrlForCrawlerIsReadOnly;
        private bool _buttonSelectFolderIsEnabled;
        private bool _buttonRunCrawlerIsEnabled;

        private bool _radioBtnSearchAllPagesIsChecked;
        private bool _radioBtnSearchTotalPagesIsChecked;


        //MODAL
        private string _modalControlVisibility;
        private string _modalControlTitle;
        private string _modalControlText;

        private string _modalControlBtnCancelVisibility;
        private string _modalControlBtnOkVisibility;
        private string _modalControlBtnYesVisibility;
        private string _modalControlBtnNoVisibility;
     

        //Public Properties
        #region BEGIN Public Properties
        public string CrawlerViewVisibility
        {
            get { return _crawlerViewVisibility; }
            set{ _crawlerViewVisibility = value;
                OnPropertyChanged(nameof(CrawlerViewVisibility));}
        }

        public string TextBoxUrlForCrawler
        {
            get { return _textBoxUrlForCrawler ?? string.Empty; }
            set { _textBoxUrlForCrawler = value;
                TextBlockUrlWarning = string.Empty;
            OnPropertyChanged(nameof(TextBoxUrlForCrawler));}
        }

        public bool TextBoxTotalPagesIsReadOnly
        {
            get { return _textBoxTotalPagesIsReadOnly; }
            set { _textBoxTotalPagesIsReadOnly = value;
            OnPropertyChanged(nameof(TextBoxTotalPagesIsReadOnly));}
        }

        public string TextBlockUrlWarning
        {
            get { return _textBlockUrlWarning ?? string.Empty; }
            set{ _textBlockUrlWarning = value;
                OnPropertyChanged(nameof(TextBlockUrlWarning));}
        }

        public string TextBlockPagesSelectedWarning
        {
            get { return _textBlockPagesSelectedWarning ?? string.Empty; }
            set { _textBlockPagesSelectedWarning = value;
            OnPropertyChanged(nameof(TextBlockPagesSelectedWarning));}
        }

        public string TextBlockSearchTypeWarning
        {
            get { return _textBlockSearchTypeWarning ?? string.Empty; }
            set { _textBlockSearchTypeWarning = value;
                OnPropertyChanged(nameof(TextBlockSearchTypeWarning)); }
        }

        public string TextBlockFolderSelectorWarning
        {
            get { return _textBlockFolderSelectorWarning ?? string.Empty;}
            set { _textBlockFolderSelectorWarning = value;
            OnPropertyChanged (nameof(TextBlockFolderSelectorWarning));}
        }


        public string TextBoxTotalPages
        {
            get { return _textBoxTotalPages ?? string.Empty; } 
            set {
                string tempVal = string.Empty;
                 foreach (char item in value)
                 {
                    var info = item;
                    if (IsDigitValue(item))
                    {
                        tempVal += item;
                       
                    }
                 }
                _textBoxTotalPages = tempVal;
                OnPropertyChanged(nameof(TextBoxTotalPages));
                TextBlockPagesSelectedWarning = string.Empty;
            }
        }

        public string TextBoxSelectedFolderPath
        {
            get { return _textBoxSelectedFolderPath;  }
            set { _textBoxSelectedFolderPath = value;
            OnPropertyChanged(nameof(TextBoxSelectedFolderPath));}
        }

        public bool TextBoxUrlForCrawlerIsReadOnly
        {
            get { return _textBoxUrlForCrawlerIsReadOnly; }
            set { _textBoxUrlForCrawlerIsReadOnly = value;
                OnPropertyChanged(nameof(TextBoxUrlForCrawlerIsReadOnly)); }
        }

        public bool ButtonSelectFolderIsEnabled
        {
            get { return _buttonSelectFolderIsEnabled; }
            set { _buttonSelectFolderIsEnabled = value;
                OnPropertyChanged(nameof(ButtonSelectFolderIsEnabled)); }
        }

        public bool ButtonRunCrawlerIsEnabled
        {
            get { return _buttonRunCrawlerIsEnabled; }
            set { _buttonRunCrawlerIsEnabled = value;
                OnPropertyChanged(nameof(ButtonRunCrawlerIsEnabled)); }
        }

        public bool RadioBtnSearchAllPagesIsChecked
        {
            get { return _radioBtnSearchAllPagesIsChecked; }
            set { _radioBtnSearchAllPagesIsChecked = value;
                OnPropertyChanged(nameof(RadioBtnSearchAllPagesIsChecked)); }
        }

        public bool RadioBtnSearchTotalPagesIsChecked
        {
            get { return _radioBtnSearchTotalPagesIsChecked; }
            set { _radioBtnSearchTotalPagesIsChecked = value;
            OnPropertyChanged(nameof(RadioBtnSearchTotalPagesIsChecked)); }
        }

        public string ModalControlVisibility
        {
            get { return _modalControlVisibility; }
            set { _modalControlVisibility = value;
                OnPropertyChanged(nameof(ModalControlVisibility));
            }
        }

        public string ModalControlTitle
        {
            get { return _modalControlTitle ?? string.Empty; }
            set { _modalControlTitle = value;
            OnPropertyChanged(nameof(ModalControlTitle));}
        }

        public string ModalControlText
        {
            get { return _modalControlText ?? string.Empty; }
            set { _modalControlText = value;
                OnPropertyChanged(nameof(ModalControlText)); }
        }

        public string ModalControlBtnCancelVisibility
        {
            get { return _modalControlBtnCancelVisibility ?? string.Empty; }
            set { _modalControlBtnCancelVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnCancelVisibility));}
        }

        public string ModalControlBtnOkVisibility
        {
            get { return _modalControlBtnOkVisibility ?? string.Empty; }
            set { _modalControlBtnOkVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnOkVisibility));
            }
        }

        public string ModalControlBtnYesVisibility
        {
            get { return _modalControlBtnYesVisibility; }
            set { _modalControlBtnYesVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnYesVisibility));
            }
        }

        public string ModalControlBtnNoVisibility
        {
            get { return _modalControlBtnNoVisibility; }
            set { _modalControlBtnNoVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnNoVisibility));
            }
        }



        #endregion END Public Properties


        //Commands
        #region BEGIN Commands 
        public ICommand GoBackButtonCommand { get; set; }
        public ICommand SearchAllPagesChecked { get; }
        public ICommand SearchTotalPagesChecked { get; }
        public ICommand SelectFolderCommand { get; }
        public ICommand RunCrawlerCommand { get; }
        public ICommand ButtonOkClickedCommand { get; }


        #endregion END Commands



        public CrawlerViewModel(IDataCollectionManager dataCollectionManager, IInputValidator inputValidator)
        {
            
            SetClearAllWarnings();
            SetModalInitialState();
            SetInputsInitialState();

            _dataCollectionManager = dataCollectionManager;
            _inputValidator = inputValidator;

            SearchAllPagesChecked = new CommandBaseViewModel(ExecuteSearchAllPagesChecked);
            SearchTotalPagesChecked = new CommandBaseViewModel(ExecuteSearchTotalPagesChecked);
            SelectFolderCommand = new CommandBaseViewModel(ExecuteSelectFolderCommand);
            RunCrawlerCommand = new CommandBaseViewModel(ExecuteRunCrawlerCommand);
            ButtonOkClickedCommand = new CommandBaseViewModel(ExecuteButtonOkClickedCommand);
        }

        private void SetInputsInitialState()
        {            
            CrawlerViewVisibility = CustomConstants.VISIBLE;
            TextBoxSelectedFolderPath = string.Empty;

            TextBoxUrlForCrawler = string.Empty;
            TextBoxUrlForCrawlerIsReadOnly = false;

            TextBoxTotalPages = string.Empty;
            TextBoxTotalPagesIsReadOnly = true;
            RadioBtnSearchAllPagesIsChecked = false;
            RadioBtnSearchTotalPagesIsChecked = false;
            
            

            ButtonSelectFolderIsEnabled = true;
            ButtonRunCrawlerIsEnabled = true;
           
        }
        private void SetModalInitialState()
        {
            ModalControlVisibility = CustomConstants.COLLAPSED;
            ModalControlTitle = CustomConstants.CRAWLER_REPORT;
            ModalControlText = string.Empty;

            ModalControlBtnCancelVisibility = CustomConstants.COLLAPSED;
            ModalControlBtnOkVisibility = CustomConstants.VISIBLE;
            ModalControlBtnYesVisibility = CustomConstants.COLLAPSED;
            ModalControlBtnNoVisibility = CustomConstants.COLLAPSED;
        }
        private void SetClearAllWarnings()
        {
            TextBlockUrlWarning = string.Empty;
            TextBlockPagesSelectedWarning = string.Empty;
            TextBlockSearchTypeWarning = string.Empty;
            TextBlockFolderSelectorWarning = string.Empty;
        }

        private void ExecuteSearchAllPagesChecked(object obj)
        {
            TextBoxTotalPagesIsReadOnly = true;
            TextBlockSearchTypeWarning = string.Empty;            
        }

        private void ExecuteSearchTotalPagesChecked(object obj)
        {
            TextBoxTotalPagesIsReadOnly = false;
            TextBlockSearchTypeWarning = string.Empty;
        }

        private void ExecuteSelectFolderCommand(object obj)
        {
            TextBoxSelectedFolderPath = string.Empty;
            TextBlockFolderSelectorWarning = string.Empty;
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult folderBrowserResult = folderBrowserDialog.ShowDialog();

                if (folderBrowserResult == DialogResult.OK && 
                    !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath) && 
                    !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    TextBoxSelectedFolderPath = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void ExecuteRunCrawlerCommand(object obj)
        {
           
            SetClearAllWarnings();

            var inputsAreValid = ResolveCrawlInputsValidation();
            if (!inputsAreValid) { return; }

            TextBoxUrlForCrawlerIsReadOnly = true;
            TextBoxTotalPagesIsReadOnly = true;
            ButtonSelectFolderIsEnabled = false;
            ButtonRunCrawlerIsEnabled = false;


            PresentationCrawlReportDelegate presentationReportCallback = new PresentationCrawlReportDelegate(GetCrawlReportResult);
            WebCrawlerInfo webCrawlerInfo = new WebCrawlerInfo()
            {
                Uri = new Uri(TextBoxUrlForCrawler),
                TotalPagesToSearch = int.Parse(TextBoxTotalPages),
                ReportFolderFullPath = TextBoxSelectedFolderPath,
                PresentationReportCallback = presentationReportCallback
            };

            _dataCollectionManager.RunCrawler(webCrawlerInfo);
        }

        private void ExecuteButtonOkClickedCommand(object obj)
        {
            SetClearAllWarnings();
            SetModalInitialState();
            SetInputsInitialState();
        }


        #region Helper Methods
        private bool IsDigitValue(char keyChar)
        {
            if(!char.IsControl(keyChar) && !char.IsDigit(keyChar))
            {
                return false;
            }
            return true;
        }

        private bool ResolveCrawlInputsValidation()
        {
            InputsForValidation crawlInputsForValidation = new InputsForValidation()
            {
                UrlText = TextBoxUrlForCrawler,
                TotalPagesForSearchText = TextBoxTotalPages,
                RadioBtnSearchAllPagesChecked = RadioBtnSearchAllPagesIsChecked,
                RadioBtnSearchCountChecked = RadioBtnSearchTotalPagesIsChecked,
                ReportFolder = TextBoxSelectedFolderPath
            };

            CrawlInputValidationReport crawlInputValidationReport = _inputValidator.ValidateCrawlInputs(crawlInputsForValidation);

            if (!crawlInputValidationReport.AllCrawlInputsAreValid)
            {
                TextBlockUrlWarning = crawlInputValidationReport.UrlLabelReport;
                TextBlockSearchTypeWarning = crawlInputValidationReport.OptionsLabelReport;
                TextBlockPagesSelectedWarning = 
                    (!string.IsNullOrEmpty(TextBlockSearchTypeWarning) || TextBoxTotalPagesIsReadOnly) ?
                    string.Empty : 
                    crawlInputValidationReport.PagesCountLabelReport;

                TextBlockFolderSelectorWarning = crawlInputValidationReport.SelectedFolderReport;

                return false;
            }

            return true;
        }

        private void GetCrawlReportResult(string reportResult)
        {
            Task.Factory.StartNew(() => {
                ButtonRunCrawlerIsEnabled = true;
                ModalControlText = reportResult;
                ModalControlBtnOkVisibility = CustomConstants.VISIBLE;
                ModalControlVisibility = CustomConstants.VISIBLE;
            });
        }

        #endregion Helper Methods

    }
}
