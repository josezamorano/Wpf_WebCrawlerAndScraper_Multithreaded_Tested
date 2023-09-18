using PresentationLayer.Utils.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace PresentationLayer.MVVM.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView(IMainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            this.DataContext = mainWindowViewModel;

            SetInitialWindowDimensions();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void SetInitialWindowDimensions()
        {
            const int snugContentWidth = 500;
            const int snugContentHeight = 650;

            var horizontalBorderHeight = SystemParameters.ResizeFrameHorizontalBorderHeight;
            var verticalBorderWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
            var captionHeight = SystemParameters.CaptionHeight;

            this.Width = snugContentWidth + 2 * verticalBorderWidth;
            this.Height = snugContentHeight + captionHeight + 2 * horizontalBorderHeight;

        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWdn, int wMsg, int wParam, int lParam); 
            
        private void panelWindowControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161,2,0);
            
           

        }

        private void panelWindowControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
