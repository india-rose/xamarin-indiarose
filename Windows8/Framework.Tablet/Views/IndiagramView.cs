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
        private readonly TextBlock _textBlock;
        private readonly Image _image;
        private readonly StackPanel _redRect;

        public readonly DependencyProperty TextColorProperty;

        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        private void RefreshColor(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            _textBlock.Foreground = TextColor;
        }
        public  readonly DependencyProperty IndiagramProperty;

        public Indiagram Indiagram
        {
            get { return (Indiagram)GetValue(IndiagramProperty); }
            set
            {
                SetValue(IndiagramProperty, value);
            }
        }
        public IndiagramView()
        {
            TextColorProperty = DependencyProperty.Register(
                "TextColor", typeof(SolidColorBrush), typeof(IndiagramView), new PropertyMetadata(default(SolidColorBrush), RefreshColor));

            Orientation = Orientation.Vertical;
            IndiagramProperty = DependencyProperty.Register(
                "Indiagram", typeof(Indiagram), typeof(IndiagramView), new PropertyMetadata(default(Indiagram), RefreshDisplay));
            var indiaSize = SettingsService.IndiagramDisplaySize;
            var margin = indiaSize / 10;
            Width = indiaSize + 2 * margin;
            Children.Clear();
            _image = new Image()
            {
                Margin = new Thickness(0, margin, 0, 0),
                Height = SettingsService.IndiagramDisplaySize,
                Width = SettingsService.IndiagramDisplaySize,
            };
            _textBlock = new TextBlock()
            {
                Margin = new Thickness(margin, 0, margin, 0),
                FontSize = SettingsService.FontSize,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _redRect = new StackPanel()
            {
                Margin = new Thickness(0, margin, 0, 0),
                Height = SettingsService.IndiagramDisplaySize,
                Width = SettingsService.IndiagramDisplaySize,
                Background = new SolidColorBrush(Colors.Red)
            };
            Children.Insert(0,_image);
            Children.Add(_textBlock);
        }

        protected virtual void RefreshDisplay(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!string.IsNullOrEmpty(Indiagram.ImagePath))
            {
                if (Children[0] != _image)
                {
                    Children.RemoveAt(0);
                    Children.Insert(0, _image);
                }
                _image.Source = new BitmapImage(new Uri(Indiagram.ImagePath, UriKind.Absolute));
            }
            else
            {
                Children.RemoveAt(0);
                Children.Insert(0, _redRect);
            }
            if (!Indiagram.IsEnabled)
            {
                _image.Opacity = 0.5;
                _redRect.Opacity = 0.5;
            }
            else
            {
                _image.Opacity = 1.0;
                _redRect.Opacity = 1.0;
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