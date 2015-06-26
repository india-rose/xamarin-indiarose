using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class TabletPreviewView : Grid
    {

        #region Properties

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(
            "ButtonStyle", typeof(Style), typeof(TabletPreviewView), new PropertyMetadata(default(Style), RefreshStyle));

        private static void RefreshStyle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshStyle();
        }

        private void RefreshStyle()
        {
            _topButton.Style = ButtonStyle;
            _bottomButton.Style = ButtonStyle;
        }

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty IndiagramSizeProperty = DependencyProperty.Register(
            "IndiagramSize", typeof(int), typeof(TabletPreviewView), new PropertyMetadata(default(int), RefreshSize));

        public int IndiagramSize
        {
            get { return (int)GetValue(IndiagramSizeProperty); }
            set { SetValue(IndiagramSizeProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register(
            "Percentage", typeof(int), typeof(TabletPreviewView), new PropertyMetadata(default(int), RefreshSize));

        private static void RefreshSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshSize();
        }

        public int Percentage
        {
            get { return (int)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty TopAreaColorProperty = DependencyProperty.Register(
            "TopAreaColor", typeof(SolidColorBrush), typeof(TabletPreviewView), new PropertyMetadata(default(SolidColorBrush), RefreshTopButtonColor));

        private static void RefreshTopButtonColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshTopButtonColor();
        }

        public void RefreshTopButtonColor()
        {
            _topButton.Background = TopAreaColor;
        }

        public SolidColorBrush TopAreaColor
        {
            get { return (SolidColorBrush)GetValue(TopAreaColorProperty); }
            set { SetValue(TopAreaColorProperty, value); }
        }

        public static readonly DependencyProperty BottomAreaColorProperty = DependencyProperty.Register(
            "BottomAreaColor", typeof(SolidColorBrush), typeof(TabletPreviewView), new PropertyMetadata(default(SolidColorBrush), RefreshBotButtonColor));

        private static void RefreshBotButtonColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshBotButtonColor();
        }

        private void RefreshBotButtonColor()
        {
            _bottomButton.Background = BottomAreaColor;
        }

        public SolidColorBrush BottomAreaColor
        {
            get { return (SolidColorBrush)GetValue(BottomAreaColorProperty); }
            set { SetValue(BottomAreaColorProperty, value); }
        }

        public static readonly DependencyProperty TopAreaCommandProperty = DependencyProperty.Register(
            "TopAreaCommand", typeof(ICommand), typeof(TabletPreviewView), new PropertyMetadata(default(ICommand)));

        public ICommand TopAreaCommand
        {
            get { return (ICommand)GetValue(TopAreaCommandProperty); }
            set { SetValue(TopAreaCommandProperty, value); }
        }

        public static readonly DependencyProperty BottomAreaCommandProperty = DependencyProperty.Register(
            "BottomAreaCommand", typeof(ICommand), typeof(TabletPreviewView), new PropertyMetadata(default(ICommand)));

        public ICommand BottomAreaCommand
        {
            get { return (ICommand)GetValue(BottomAreaCommandProperty); }
            set { SetValue(BottomAreaCommandProperty, value); }
        }
        #endregion

        private readonly Image _tabletImage = new Image();
        private readonly Button _topButton = new Button();
        private readonly Button _bottomButton = new Button();
        private readonly StackPanel _stackPanel = new StackPanel();
        private readonly Grid _topGrid = new Grid();
        private readonly Grid _botGrid = new Grid();
        private readonly Image _nextImage = new Image();
        private readonly Image _catImage = new Image();
        private readonly Image _rabbidImage = new Image();
        private readonly Image _playImage = new Image();

        public TabletPreviewView()
        {
            _catImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/chat.png"));
            _playImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/playbutton.png"));
            _rabbidImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/lapin.png"));
            _nextImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/nextarrow.png"));
            _tabletImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/tab.png"));
            _tabletImage.Stretch = Stretch.Uniform;
            _bottomButton.HorizontalAlignment = HorizontalAlignment.Center;
            _bottomButton.Tapped += _bottomButton_Tapped;
            _topButton.HorizontalAlignment = HorizontalAlignment.Center;
            _topButton.Tapped += _topButton_Tapped;
            _stackPanel.Orientation = Orientation.Vertical;


            Children.Add(_tabletImage);
            Children.Add(_stackPanel);
            _stackPanel.Children.Add(_topGrid);
            _stackPanel.Children.Add(_botGrid);

            SizeChanged += TabletPreview_SizeChanged;
        }

        void _bottomButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (BottomAreaCommand != null && BottomAreaCommand.CanExecute(null))
            {
                BottomAreaCommand.Execute(null);
            }
        }

        void _topButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TopAreaCommand != null && TopAreaCommand.CanExecute(null))
            {
                TopAreaCommand.Execute(null);
            }
        }

        void TabletPreview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshSize();
        }

        private void RefreshSize()
        {
            var height = _tabletImage.ActualHeight;
            var width = _tabletImage.ActualWidth;

            //ratio from initial image without border
            height *= 0.81;
            width *= 0.79;
            
            //empty area
            _stackPanel.Height = height;
            _stackPanel.Width = width;

            //ratio to reduce indiagram for the preview
            var ratiorealscreen = height / LazyResolver<IScreenService>.Service.Height;
            var indiasize = IndiagramSize * ratiorealscreen;
            //add a small border
            indiasize *= 1.2;
            RefreshIndiaSize(indiasize);
            var nbIndia = (int)(width/indiasize);

            _botGrid.ColumnDefinitions.Clear();
            _topGrid.ColumnDefinitions.Clear();
            //create cells to place preview of indiagram
            for (int i = 0; i < nbIndia; i++)
            {
                _botGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                _topGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            try
            {
                AddGrid(nbIndia);
            }
            catch (ArgumentException e)
            {
            }
            //define height and width of button
            _topButton.Height = height * (Percentage / 100.0);
            _topButton.Width = width;
            _bottomButton.Width = width;
            _bottomButton.Height = height * (1 - (Percentage / 100.0));
        }

        private void RefreshIndiaSize(double indiasize)
        {
            _playImage.Height = indiasize;
            _nextImage.Height = indiasize;
            _rabbidImage.Height = indiasize;
            _catImage.Height = indiasize;

            _playImage.Width = indiasize;
            _nextImage.Width = indiasize;
            _rabbidImage.Width = indiasize;
            _catImage.Width = indiasize;

        }

        private void AddGrid(int nbIndia)
        {
            //give to the large button the entire span of table
            SetColumnSpan(_bottomButton, nbIndia);
            SetColumnSpan(_topButton, nbIndia);
            //if we can, add cat image
            if (nbIndia > 1)
                SetColumn(_catImage, 0);
            //if we can, add rabbid image
            if (nbIndia > 2)
                SetColumn(_rabbidImage, 1);
            //if we can, add play and next image
            if (nbIndia > 0)
            {
                SetColumn(_nextImage, nbIndia - 1);
                SetColumn(_playImage, nbIndia - 1);
            }

            _topGrid.Children.Clear();
            _botGrid.Children.Clear();

            _topGrid.Children.Add(_topButton);
            _topGrid.Children.Add(_rabbidImage);
            _topGrid.Children.Add(_catImage);
            _topGrid.Children.Add(_nextImage);

            _botGrid.Children.Add(_bottomButton);
            _botGrid.Children.Add(_playImage);


        }
    }
}
