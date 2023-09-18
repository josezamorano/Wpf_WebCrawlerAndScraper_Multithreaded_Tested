using Autofac;
using DataAccessLayer.IOFiles;
using DataAccessLayer.RemoteWebSites;
using DataAccessLayer.Utils.Interfaces;
using DomainLayer;
using DomainLayer.Utils.Interfaces;
using PresentationLayer.CustomUserControls;
using PresentationLayer.MVVM.ViewModels;
using PresentationLayer.MVVM.Views;
using PresentationLayer.Utils.Interfaces;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IMainWindowViewModel _mainWindowViewModel;
        public App()
        {
            ConfigureDependencyInjectionContainer();
        }

        private void ConfigureDependencyInjectionContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //Service Layer
            builder.RegisterType<InputValidator>().As<IInputValidator>();
            builder.RegisterType<LinkInspector>().As<ILinkInspector>();
            builder.RegisterType<ScrapingFileStateManager>().As<IScrapingFileStateManager>();

            //Data Access Layer
            builder.RegisterType<CsvFileProvider>().As<ICsvFileProvider>();
            builder.RegisterType<DirectoryProvider>().As<IDirectoryProvider>();
            builder.RegisterType<DownloadProvider>().As<IDownloadProvider>();
            builder.RegisterType<FileActivityManager>().As<IFileActivityManager>();
            builder.RegisterType<FileHelper>().As<IFileHelper>();
            builder.RegisterType<PathProvider>().As<IPathProvider>();
            builder.RegisterType<TextFileProvider>().As<ITextFileProvider>();

            //Domain Layer
            builder.RegisterType<DataCollectionManager>().As<IDataCollectionManager>();
            builder.RegisterType<HtmlParser>().As<IHtmlParser>();
            builder.RegisterType<WebSiteCrawler>().As<IWebSiteCrawler>();
            builder.RegisterType<WebSiteScraper>().As<IWebSiteScraper>();

            //Presentation Layer
            builder.RegisterType<MainWindowViewModel>().As<IMainWindowViewModel>().SingleInstance();
            builder.RegisterType<CrawlerViewModel>().As<ICrawlerViewModel>().SingleInstance();
            builder.RegisterType<ScraperViewModel>().As<IScraperViewModel>().SingleInstance();
            builder.RegisterType<AllTextFilesListBox>().As<IAllTextFilesListBox>().SingleInstance();


            IContainer newContainer = builder.Build();
            ILifetimeScope newScope = newContainer.BeginLifetimeScope();

            _mainWindowViewModel = newScope.Resolve<IMainWindowViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindowView mainWindowView = new MainWindowView(_mainWindowViewModel);
            if(mainWindowView != null) 
            {
                mainWindowView.Show();
            }
            
        }

    }
}
