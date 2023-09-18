using DomainLayer.Models;
using DomainLayer.Utils.Interfaces;
using PresentationLayer.MVVM.Base;
using PresentationLayer.Utils.Interfaces;
using ServiceLayer.Constants;
using ServiceLayer.Interfaces;
using ServiceLayer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static ServiceLayer.DelegateTypes.CustomDelegate;

namespace PresentationLayer.MVVM.ViewModels
{
    public class ScraperViewModel : NotifyBaseViewModel , IScraperViewModel
    {
        IDataCollectionManager _dataCollectionManager;
        IInputValidator _inputValidator;
        IScrapingFileStateManager _scrapingFileStateManager;

        private string _scraperViewVisibility;
        private string _textBoxSelectedFolderPath;
        private bool _buttonSelectFolderIsEnabled;


        private string _textBlockFolderSelectorWarning;
        private string _textBlockTotalSelectedFilesWarninng;
        private string _textBlockXPathExpressionWarning;
        private bool _textBoxXPathExpressionIsReadonly;

        private string _textBoxXPathExpression;

        private string _sourceScrapingFilesListBoxVisibility;
        private string _customControlSelectedFolderPath;
        private List<SourceScrapingFile> _customControlAllFiles;
        private bool _customControlAllFilesSelected;

        private string _textBlockTotalCountSelectedFiles;
        private bool _buttonRunScraperIsEnabled;

        private List<SourceScrapingFile> _totalSelectedTxtFiles;


        private string _modalControlVisibility;
        private string _modalControlTitle;
        private string _modalControlText;
        private string _modalControlBtnCancelVisibility;
        private string _modalControlBtnOkVisibility;
        private string _modalControlBtnYesVisibility;
        private string _modalControlBtnNoVisibility;


        #region Public Properties
        public string ScraperViewVisibility
        {
            get { return _scraperViewVisibility; }
            set { _scraperViewVisibility = value;
            OnPropertyChanged(nameof(ScraperViewVisibility));}
        }

        public string TextBoxSelectedFolderPath
        {
            get { return _textBoxSelectedFolderPath ?? string.Empty;}
            set { _textBoxSelectedFolderPath = value;
                OnPropertyChanged(nameof(TextBoxSelectedFolderPath)); }
        }

        public bool ButtonSelectFolderIsEnabled
        {
            get { return _buttonSelectFolderIsEnabled;}
            set { _buttonSelectFolderIsEnabled = value;
                OnPropertyChanged(nameof(ButtonSelectFolderIsEnabled));
            }
        }

        public string TextBlockFolderSelectorWarning
        {
            get { return _textBlockFolderSelectorWarning ?? string.Empty; }
            set { _textBlockFolderSelectorWarning = value;
            OnPropertyChanged(nameof(TextBlockFolderSelectorWarning));}
        }

        public string TextBlockTotalSelectedFilesWarninng
        {
            get { return _textBlockTotalSelectedFilesWarninng; }
            set { _textBlockTotalSelectedFilesWarninng = value;
                OnPropertyChanged(nameof(TextBlockTotalSelectedFilesWarninng));
            }
        }

        public string TextBlockXPathExpressionWarning
        {
            get { return _textBlockXPathExpressionWarning; }
            set { _textBlockXPathExpressionWarning = value; 
                OnPropertyChanged(nameof(TextBlockXPathExpressionWarning)); }
        }

        public string TextBoxXPathExpression
        {
            get { return (_textBoxXPathExpression ?? string.Empty); }
            set { _textBoxXPathExpression = value;
                if (!string.IsNullOrEmpty(TextBlockXPathExpressionWarning))
                {
                    TextBlockXPathExpressionWarning = string.Empty;
                }
               
                OnPropertyChanged(nameof(TextBoxXPathExpression));}
        }

        public bool TextBoxXPathExpressionIsReadonly
        {
            get { return _textBoxXPathExpressionIsReadonly; }
            set { _textBoxXPathExpressionIsReadonly = value;
                OnPropertyChanged(nameof(TextBoxXPathExpressionIsReadonly));
            }
        }

        public string CustomControlSelectedFolderPath
        {
            get { return _customControlSelectedFolderPath ?? string.Empty; }
            set { _customControlSelectedFolderPath = value;
            OnPropertyChanged(nameof(CustomControlSelectedFolderPath));}
        }

        public string SourceScrapingFilesListBoxVisibility
        {
            get { return _sourceScrapingFilesListBoxVisibility; }
            set { _sourceScrapingFilesListBoxVisibility = value;
                OnPropertyChanged(nameof(SourceScrapingFilesListBoxVisibility)); }
        }

        public List<SourceScrapingFile> CustomControlAllFiles
        {
            get { return _customControlAllFiles; }
            set { _customControlAllFiles = value;
                OnPropertyChanged(nameof(CustomControlAllFiles)); }
        }

        public bool CustomControlAllFilesSelected
        {
            get { return _customControlAllFilesSelected; }
            set{_customControlAllFilesSelected = value;
                SetScrapingFilesState(_customControlAllFilesSelected);
                OnPropertyChanged(nameof(CustomControlAllFilesSelected));}
        }

        public string TextBlockTotalCountSelectedFiles
        {
            get { return _textBlockTotalCountSelectedFiles; }
            set { _textBlockTotalCountSelectedFiles = value;
            OnPropertyChanged(nameof(TextBlockTotalCountSelectedFiles));}
        }

        public bool ButtonRunScraperIsEnabled
        {
            get { return _buttonRunScraperIsEnabled; }
            set { _buttonRunScraperIsEnabled = value;
                OnPropertyChanged(nameof(ButtonRunScraperIsEnabled));}
        }


        public string ModalControlVisibility
        {
            get { return _modalControlVisibility; }
            set { _modalControlVisibility = value;
            OnPropertyChanged(nameof(ModalControlVisibility));}
        }

        public string ModalControlTitle
        {
            get { return _modalControlTitle; }
            set { _modalControlTitle = value;
            OnPropertyChanged(nameof(ModalControlTitle));}
        }

        public string ModalControlText
        {
            get { return _modalControlText; }
            set { _modalControlText = value;
            OnPropertyChanged(nameof(ModalControlText));}
        }

        public string ModalControlBtnCancelVisibility
        {
            get { return _modalControlBtnCancelVisibility; }
            set { _modalControlBtnCancelVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnCancelVisibility));
            }
        }

        public string ModalControlBtnOkVisibility
        {
            get { return _modalControlBtnOkVisibility; }
            set { _modalControlBtnOkVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnOkVisibility));
            }
        }

        public string ModalControlBtnYesVisibility
        {
            get { return _modalControlBtnYesVisibility; }
            set { _modalControlBtnYesVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnYesVisibility)); }
        }

        public string ModalControlBtnNoVisibility
        {
            get { return _modalControlBtnNoVisibility; }
            set { _modalControlBtnNoVisibility = value;
                OnPropertyChanged(nameof(ModalControlBtnNoVisibility)); }
        }

        #endregion Public Properties

        #region Commands

        public ICommand GoBackButtonCommand { get; set; }
        public ICommand SelectFolderCommand { get;  }
        public ICommand OpenListBoxAllTextFiles { get;  }
        public ICommand SelectScrapingFilesCommand { get; }
        public ICommand SaveAndGoBackToViewCommand { get; }
        public ICommand RunScraperCommand { get; }

        public ICommand ButtonOkClickedCommand { get; }

        #endregion Commands

        public ScraperViewModel(IDataCollectionManager dataCollectionManager, 
                                IInputValidator inputValidator,
                                IScrapingFileStateManager scrapingFileStateManager
            )
        {

            _dataCollectionManager = dataCollectionManager;
            _inputValidator = inputValidator;
            _scrapingFileStateManager = scrapingFileStateManager;

            SetClearAllWarnings();
            SetModalInitialState();
            SetInputsInitialState();

             _customControlAllFiles = new List<SourceScrapingFile>();
            _totalSelectedTxtFiles = new List<SourceScrapingFile>(); 
                        

            SelectFolderCommand = new CommandBaseViewModel(ExecuteSelectFolderCommand);
            SelectScrapingFilesCommand = new CommandBaseViewModel(ExecuteSelectScrapingFilesCommand);
            SaveAndGoBackToViewCommand = new CommandBaseViewModel(ExecuteSaveAndGoBackToViewCommand);
            RunScraperCommand = new CommandBaseViewModel(ExecuteRunScraperCommand);
            ButtonOkClickedCommand = new CommandBaseViewModel(ExecuteButtonOkClickedCommand);

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
                    List<string> allFiles = _dataCollectionManager.GetAllFilesInSelectedDirectory(folderBrowserDialog.SelectedPath);
                    CustomControlSelectedFolderPath = folderBrowserDialog.SelectedPath;
                    foreach (string item in allFiles)
                    {
                        SourceScrapingFile sourceFile = new SourceScrapingFile() { FilePath = item, IsSelected = false };
                        CustomControlAllFiles.Add(sourceFile);
                    }
                }
            }
        }

        private void ExecuteSelectScrapingFilesCommand(object obj)
        {
            TextBlockTotalSelectedFilesWarninng = string.Empty;
            SourceScrapingFilesListBoxVisibility = CustomConstants.VISIBLE;
        }
        
        private void ExecuteSaveAndGoBackToViewCommand(object obj)
        {
            _totalSelectedTxtFiles = CustomControlAllFiles.Where(a=>a.IsSelected== true).ToList();
            string messageCount = (_totalSelectedTxtFiles.Count > 1) ? " files were " : " file was ";
            string message = "A total of  "+ _totalSelectedTxtFiles.Count.ToString()+ ". TXT"+ messageCount + "selected in the Folder.";
            TextBlockTotalCountSelectedFiles = message;
            SourceScrapingFilesListBoxVisibility = CustomConstants.COLLAPSED;
        }

        private void ExecuteRunScraperCommand(object obj)
        {
            
            var inputsAreValid = ResolveScrapeInputsValidation();
            if (!inputsAreValid) { return; }

            SetClearAllWarnings();
            SetModalInitialState();
            DisableAllInputs();
            Queue<string> allCheckedItems = GetAllCheckedTextFiles();
            PresentationScrapeReportDelegate reportCallback = new PresentationScrapeReportDelegate(GetScrapeReportResult);
            WebScraperInfo webScrapeInfo = new WebScraperInfo()
            {
                ScrapingFolderFullPath = TextBoxSelectedFolderPath,
                AllTextFilesToScrape = allCheckedItems,
                ScrapeReportCallback = reportCallback,
                XPathExpression = TextBoxXPathExpression,
            };
            _dataCollectionManager.RunScraper(webScrapeInfo);
        }


        private void ExecuteButtonOkClickedCommand(object obj)
        {
            SetClearAllWarnings();
            SetModalInitialState();
            SetInputsInitialState();
        }

        #region Helper Methods

        private void SetClearAllWarnings()
        {
            TextBlockFolderSelectorWarning = string.Empty;
            TextBlockXPathExpressionWarning = string.Empty;
            TextBlockTotalSelectedFilesWarninng = string.Empty;
        }

        private void SetModalInitialState()
        {

            ModalControlVisibility = CustomConstants.COLLAPSED;
            ModalControlTitle = CustomConstants.SCRAPER_REPORT;
            ModalControlText = string.Empty;
            ModalControlBtnCancelVisibility = CustomConstants.COLLAPSED;
            ModalControlBtnOkVisibility = CustomConstants.COLLAPSED;
            ModalControlBtnYesVisibility = CustomConstants.COLLAPSED;
            ModalControlBtnNoVisibility = CustomConstants.COLLAPSED;    
        }

        private void SetInputsInitialState()
        {
            SourceScrapingFilesListBoxVisibility = CustomConstants.COLLAPSED;
            ButtonSelectFolderIsEnabled = true;
            ButtonRunScraperIsEnabled = true;
            TextBoxXPathExpressionIsReadonly = false;

            TextBoxSelectedFolderPath = string.Empty;
            TextBoxXPathExpression = string.Empty;
        }

        private void DisableAllInputs()
        {
            ButtonSelectFolderIsEnabled = false;
            ButtonRunScraperIsEnabled = false;
            TextBoxXPathExpressionIsReadonly = true;
            SourceScrapingFilesListBoxVisibility = CustomConstants.COLLAPSED;
            ModalControlVisibility= CustomConstants.COLLAPSED;
        }

        private void SetScrapingFilesState(bool isChecked)
        {
            List<SourceScrapingFile> updatedSourceFiles = _scrapingFileStateManager.UpdateScrapingFilesState(CustomControlAllFiles, isChecked);
            CustomControlAllFiles = updatedSourceFiles;
        }


        private bool ResolveScrapeInputsValidation()
        {
            InputsForValidation scrapeInputsForValidation = new InputsForValidation()
            {
                ScrapingFolderPath = TextBoxSelectedFolderPath,
                AllCheckedTxtFiles = GetAllCheckedTextFiles(),
                XPathExpression = TextBoxXPathExpression
            };

            ScrapeInputValidationReport scrapeInputValidationReport = _inputValidator.ValidateScrapeInputs(scrapeInputsForValidation);
            if (!scrapeInputValidationReport.AllScrapeInputsAreValid)
            {
                if (!string.IsNullOrEmpty(scrapeInputValidationReport.ScrapingFolderPathReport))
                {
                    TextBlockFolderSelectorWarning  = scrapeInputValidationReport.ScrapingFolderPathReport;
                }

                if (!string.IsNullOrEmpty(scrapeInputValidationReport.AlltxtFilesReport))
                {
                    TextBlockTotalSelectedFilesWarninng = scrapeInputValidationReport.AlltxtFilesReport;
       
                }

                if (!string.IsNullOrEmpty(scrapeInputValidationReport.XPathExpressionReport))
                {
                    TextBlockXPathExpressionWarning = scrapeInputValidationReport.XPathExpressionReport;
                }

                return false;
            }

            return true;
        }

        private Queue<string> GetAllCheckedTextFiles()
        {
            Queue<string> allCheckedItems = new Queue<string>();
            var allFiles = _totalSelectedTxtFiles;
            foreach (var itemChecked in _totalSelectedTxtFiles)
            {
                var info = (string.IsNullOrEmpty(itemChecked.FilePath)) ? string.Empty : itemChecked.FilePath;
                allCheckedItems.Enqueue(info);
            }
            return allCheckedItems;
        }
        private void GetScrapeReportResult(string reportResult)
        {
            Task.Factory.StartNew(() => { 
                ButtonRunScraperIsEnabled = true;
                SourceScrapingFilesListBoxVisibility = CustomConstants.COLLAPSED;
                ModalControlVisibility = CustomConstants.VISIBLE;
                ModalControlBtnOkVisibility = CustomConstants.VISIBLE;
                ModalControlText = reportResult;

            });
        }

        #endregion Helper Methods
    }
}
