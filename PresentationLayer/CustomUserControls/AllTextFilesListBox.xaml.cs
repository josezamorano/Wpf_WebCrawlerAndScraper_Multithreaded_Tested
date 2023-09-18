using PresentationLayer.Utils.Interfaces;
using ServiceLayer.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PresentationLayer.CustomUserControls
{
    public partial class AllTextFilesListBox : UserControl, IAllTextFilesListBox
    {

        #region Private Attributes

        private string _customControlSourceFolderPath;
        private List<SourceScrapingFile> _customControlAllScrapingFiles;
        private bool _customControlAllFilesAreChecked;

        private ICommand _saveAndGoBackCommand;


        #endregion Private Attributes



        #region Public Properties

        public string CustomControlSourceFolderPath
        {
            get { return _customControlSourceFolderPath; }
            set { _customControlSourceFolderPath = value; }
        }

        public static readonly DependencyProperty CustomControlSourceFolderPathProperty =
            DependencyProperty.Register(nameof(CustomControlSourceFolderPath), typeof(string), typeof(AllTextFilesListBox));

        public List<SourceScrapingFile> CustomControlAllScrapingFiles
        {
            get { return _customControlAllScrapingFiles; }
            set {
                _customControlAllScrapingFiles = value;
            }
        }


        public static readonly DependencyProperty CustomControlAllScrapingFilesProperty =
            DependencyProperty.Register(nameof(CustomControlAllScrapingFiles), typeof(List<SourceScrapingFile>), typeof(AllTextFilesListBox));

        public bool CustomControlAllFilesAreChecked
        {
            get { return _customControlAllFilesAreChecked; }
            set { _customControlAllFilesAreChecked = value;
            }
        }

        public static readonly DependencyProperty CustomControlAllFilesAreCheckedProperty =
          DependencyProperty.Register(nameof(CustomControlAllFilesAreChecked), typeof(bool), typeof(AllTextFilesListBox)
              , new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ICommand SaveAndGoBackCommand
        {
            get { return _saveAndGoBackCommand; }
            set { _saveAndGoBackCommand = value;}
        }

        public static readonly DependencyProperty SaveAndGoBackCommandProperty =
            DependencyProperty.Register(nameof(SaveAndGoBackCommand), typeof(ICommand), typeof(AllTextFilesListBox));

        #endregion Public Properties



        public AllTextFilesListBox()
        {
            InitializeComponent();;
        }       
    }
}
