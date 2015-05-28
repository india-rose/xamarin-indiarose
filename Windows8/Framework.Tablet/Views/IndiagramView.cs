using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class IndiagramView : StackPanel
    {
        protected ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }
        private TextBlock _textBlock;
        private Image _image;

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(SolidColorBrush), typeof(IndiagramView), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
                RefreshDisplay();
            }
        }

        public static readonly DependencyProperty IndiagramProperty = DependencyProperty.Register(
            "Indiagram", typeof (Indiagram), typeof (IndiagramView), new PropertyMetadata(default(Indiagram)));

        public Indiagram Indiagram
        {
            get { return (Indiagram) GetValue(IndiagramProperty); }
            set
            {
                SetValue(IndiagramProperty, value);
                RefreshDisplay();
            }
        }
        public IndiagramView()
        {
            Orientation=Orientation.Vertical;
            Background = new SolidColorBrush(Colors.Aquamarine);
        }

        void RefreshDisplay()
        {
            Width = SettingsService.IndiagramDisplaySize;
            Children.Clear();
            _image = new Image()
            {
                Height = SettingsService.IndiagramDisplaySize,
                Width = SettingsService.IndiagramDisplaySize,
                Source = new BitmapImage(new Uri(Indiagram.ImagePath, UriKind.Absolute))
            };
            _textBlock = new TextBlock()
            {
                Text = Indiagram.Text,
                FontSize = SettingsService.FontSize,
                Foreground = TextColor,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Children.Add(_image);
            Children.Add(_textBlock);

        }
    }
}