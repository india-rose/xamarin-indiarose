using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Framework.Converters;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class IndiagramView : StackPanel
    {
        protected static ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }
        private TextBlock _textBlock;
        private Image _image;
        private readonly int _indiaSize;
        private readonly int _margin;

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(SolidColorBrush), typeof(IndiagramView), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
                _textBlock.Foreground = TextColor;
            }
        }

        public static readonly DependencyProperty IndiagramProperty = DependencyProperty.Register(
            "Indiagram", typeof(Indiagram), typeof(IndiagramView), new PropertyMetadata(default(Indiagram)));

        public Indiagram Indiagram
        {
            get { return (Indiagram)GetValue(IndiagramProperty); }
            set
            {
                SetValue(IndiagramProperty, value);
                RefreshDisplay();
            }
        }
        public IndiagramView()
        {
            Orientation = Orientation.Vertical;
            _indiaSize = SettingsService.IndiagramDisplaySize;
            _margin = _indiaSize / 10;
            Width = _indiaSize + 2 * _margin;
            Children.Clear();
            _image = new Image()
            {
                Margin = new Thickness(0, _margin, 0, 0),
                Height = SettingsService.IndiagramDisplaySize,
                Width = SettingsService.IndiagramDisplaySize,
            };
            _textBlock = new TextBlock()
            {
                Margin = new Thickness(_margin, 0, _margin, 0),
                FontSize = SettingsService.FontSize,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Children.Add(_image);
            Children.Add(_textBlock);
        }

        void RefreshDisplay()
        {
            if (!string.IsNullOrEmpty(Indiagram.ImagePath))
                _image.Source = new BitmapImage(new Uri(Indiagram.ImagePath, UriKind.Absolute));
            else
            {
            }
            if (Indiagram.IsEnabled)
            {
            }
            if (!string.IsNullOrEmpty(Indiagram.Text))
                _textBlock.Text = Indiagram.Text;
        }

        public static int DefaultWidth
        {
            get
            {
                return (int)(SettingsService.IndiagramDisplaySize * 1.2);
            }
        }

        public static int DefaultHeight
        {
            get
            {
                return (int)(SettingsService.IndiagramDisplaySize * 1.2 + SettingsService.FontSize);
            }
        }
    }
}