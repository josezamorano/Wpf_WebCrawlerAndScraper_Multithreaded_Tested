using PresentationLayer.MVVM.Base;
using PresentationLayer.Utils.Interfaces;
using ServiceLayer.Constants;
using System.Windows.Input;

namespace PresentationLayer.MVVM.ViewModels
{
    public class MainWindowViewModel : NotifyBaseViewModel, IMainWindowViewModel
    {
        //Private Attributes
        #region Private Attributes 
        ICrawlerViewModel _crawlerViewModel;
        IScraperViewModel _scraperViewModel;

        private string _mainPanelMenuButtonsVisibility;

        private NotifyBaseViewModel _currentChildView;
        #endregion


        #region Public Properties
        public NotifyBaseViewModel CurrentChildView
        {
            get { return _currentChildView; }
            set { _currentChildView = value; 
                OnPropertyChanged(nameof(CurrentChildView)); }
        }

        public string MainPanelMenuButtonsVisibility
        {
            get { return _mainPanelMenuButtonsVisibility ?? string.Empty; }
            set { _mainPanelMenuButtonsVisibility = value;
            OnPropertyChanged(nameof(MainPanelMenuButtonsVisibility)); }
        }

        #endregion

        #region Public Commands 
        public ICommand OpenCrawlerCommand { get; }
        public ICommand OpenScraperCommand { get; }

        #endregion
        
        public MainWindowViewModel(ICrawlerViewModel crawlerViewModel, IScraperViewModel scraperViewModel)
        {
            _crawlerViewModel = crawlerViewModel;
            _scraperViewModel = scraperViewModel;

            MainPanelMenuButtonsVisibility = CustomConstants.VISIBLE;

            OpenCrawlerCommand = new CommandBaseViewModel(ExecuteOpenCrawlerCommand);
            OpenScraperCommand = new CommandBaseViewModel(ExecuteOpenScraperCommand);
            _crawlerViewModel.GoBackButtonCommand = new CommandBaseViewModel(ExecuteCrawlerGoBackButtonCommand);
            _scraperViewModel.GoBackButtonCommand = new CommandBaseViewModel(ExecuteScraperGoBackButtonCommand);
        }

        private void ExecuteOpenCrawlerCommand(object obj)
        {
            MainPanelMenuButtonsVisibility = CustomConstants.COLLAPSED;
            _crawlerViewModel.CrawlerViewVisibility = CustomConstants.VISIBLE;
            
            CurrentChildView = (NotifyBaseViewModel)_crawlerViewModel;            
        }

        private void ExecuteOpenScraperCommand(object obj)
        {
            MainPanelMenuButtonsVisibility = CustomConstants.COLLAPSED;
            _scraperViewModel.ScraperViewVisibility = CustomConstants.VISIBLE;
            CurrentChildView = (NotifyBaseViewModel) _scraperViewModel;
        }

        private void ExecuteCrawlerGoBackButtonCommand(object obj)
        {
            MainPanelMenuButtonsVisibility = CustomConstants.VISIBLE;
            _crawlerViewModel.CrawlerViewVisibility = CustomConstants.COLLAPSED;
        }

        private void ExecuteScraperGoBackButtonCommand(object obj)
        {
            MainPanelMenuButtonsVisibility= CustomConstants.VISIBLE;
            _scraperViewModel.ScraperViewVisibility = CustomConstants.COLLAPSED;
        }
    }
}
