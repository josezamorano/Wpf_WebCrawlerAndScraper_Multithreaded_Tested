   M S B A M L   `   `   `  ����L  HPresentationLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:PresentationLayer.MVVM.ViewsPresentationLayer.MVVM.Views  5   	   U/clr-namespace:PresentationLayer.MVVM.ViewModels!PresentationLayer.MVVM.ViewModels  5   	   Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466J6clr-namespace:FontAwesome.WPF;assembly=FontAwesome.WPFFontAwesome.WPF 5
   	   1    +PresentationLayer.MVVM.Views.MainWindowView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;view*clr-namespace:PresentationLayer.MVVM.Views     5   	   E	viewModel/clr-namespace:PresentationLayer.MVVM.ViewModels     5   	   .fa1$http://schemas.fontawesome.io/icons/  5   	   X TFontAwesome.Sharp, Version=6.3.0.0, Culture=neutral, PublicKeyToken=d16d1e4e568ec10fLfa_sharp=http://schemas.awesome.incremented/wpf/xaml/fontawesome.sharp  5	   	   Cfa_wpf6clr-namespace:FontAwesome.WPF;assembly=FontAwesome.WPF  5
   	     � Title$  MainWindowView��5   	   $	��300��5   	   $	��550��5   	   $	��800��5   	   $	��900��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � WindowStyle$
 None=�5   	   
������  � AllowsTransparency$
 True��5   	   '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�6      ResizeBorderThickness$ 5q�6)    � 	Resources 5       �� DataType8   2PresentationLayer.MVVM.ViewModels.CrawlerViewModel.   (PresentationLayer.MVVM.Views.CrawlerView8   2PresentationLayer.MVVM.ViewModels.ScraperViewModel.   (PresentationLayer.MVVM.Views.ScraperView��%�   (��       5      *, +)(�� 2     5       *, +)�� 5   
   "  6   .R�5        5      �� 5    
   "  6   .R�5!        5"      5$      .��5&      �� $��Disabled=�5'   
   $
��Auto=�5(   
   .��5*   
   �� 	 �� CornerRadius$	 5��5+   	   $��3q�5,   	   
 J� Opacity$
 1Q�5-   	   ��5/      �� 50       �� 
StartPoint$	 0,0&�6&    �� EndPoint$	 1,1&�67   .��51      �    panelborderWhite � Color# [  6#    � Offset$ 0Q�6M   � 52        panelborderBlue_IV# [ 6#   $
 0.35Q�6O   � 53        panelborderBlue_VI# [ 6#   $
 0.75Q�6O   � 54        panelborderBlue_I# [ 6#   $ 1Q�6N   55      ._�58      � ��59      �� 5:      $ 0.5,0&�6*   $ 0.5,1&�6=   .��5;      �   panelBackgroundBlue_I# [ 6'   � 5<        panelBackgroundBlue_II# [ 6'   $	 1.0Q�6W   5=       � RowDefinitions 5@      �� 5B      $��40 �6$   �� 5D      $
��Auto �6$   �� 5F      $��* �6$   .+�5I      �� $��0��6   
���  ��$��0q�68   �� 5J      $��1��6   
��� � �$��0q�6:   �� 5K      $��2��6   
��� ���$��0q�6;   �� 5N      -   5         ��Name$ panelWindowControlBar��5N      $��0��5O       �� Orientation$ 
Horizontal=�5P      $��RightToLeft=�5Q      
������ .+�5V      �� -   5         ��Name$ btnClose��5V        windowPanelButtonCorner#��[ 5W      .��5Y      "  FontAwesome.WPF.ImageAwesome     Icon$ Close=�6.     windowPanelIcon#��[ 6;   5Z      �� 5\      -   5        $ btnMaximize��5\        windowPanelButton#��[ 5]      .��5_        $ WindowMaximize=�6.   #��[ 6D   5`      �� 5b      -   5        $ btnMinimize��5b      #��[ 5c      .��5g        $ WindowMinimize=�6.   #��[ 6D   5h      5j      �� 5m      -   5        $ mainPanelMenuButtons��5m      $��1��5n      $ 
Horizontal=�5o      $��LeftToRight=�5p      y�5q      �� * MainPanelMenuButtonsVisibility+.+�5s      �� -   5        $ buttonOpenCrawler��5s       	 menuPanelButton#��[	 5t      ��5u      �� *OpenCrawlerCommand+ �� UpdateSourceTrigger$ PropertyChanged=�.��5w      �� -   5         ��Name$ 
lblCrawler��5w       $��Crawler��64   $��14(�6F    
 plainTextBrushNeonWhite#��[
 5x      $��Center=�5y      $��Center=�5z      5|      �� 5~      -   5        $ buttonOpenScraper��5~      #��[	 5      ��5�      �� *OpenScraperCommand+$ PropertyChanged=�.��5�      �� -	   5        $ 
lblScraper��5�       $��Scraper��64   $��14(�6G   #��[
 5�      $��Center=�5�      $��Center=�69   5�      5�      �� 5�      $��1��5�   "   ��5�   !   �� *CurrentChildView+5�   "   5�      5�      5�      5�      