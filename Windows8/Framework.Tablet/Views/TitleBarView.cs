using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class TitleBarView : StackPanel
    {
        private readonly Image _imagecategory;
        private readonly TextBlock _textblock;
        private readonly StackPanel _redRect;

        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(
            "Category", typeof(Category), typeof(TitleBarView), new PropertyMetadata(default(Category),Refresh));

        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TitleBarView;
            if (view != null) view.Refresh();
        }

        private void Refresh()
        {
            _textblock.Text = Category.Text;
            if (Category.ImagePath == null)
            {
                try
                {
                    Children.RemoveAt(0);
                }
                catch (ArgumentException) { }
                Children.Insert(0,_redRect);
            }
            else
            {
                try
                {
                    Children.RemoveAt(0);
                }
                catch (ArgumentException) { }
                _imagecategory.Source = new BitmapImage(new Uri(Category.ImagePath, UriKind.Absolute));
                Children.Insert(0, _imagecategory);
            }
        }

        public Category Category
        {
            get { return (Category)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public TitleBarView()
        {
            var settingsService = LazyResolver<ISettingsService>.Service;
            _textblock = new TextBlock()
            {
                FontSize = settingsService.FontSize,
                Foreground = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };
            _redRect = new StackPanel()
            {
                Height = 60,
                Width = 60,
                Background = new SolidColorBrush(Colors.Red)
            };
            _imagecategory = new Image()
            {
                Height = 60,
                Width = 60,
            };
            Background=new SolidColorBrush(Colors.White);
            Orientation = Orientation.Horizontal;
            const string sourcelogo = "ms-appx:///Assets/logoIndiaRose.png";
            var logo = new Image { Source = new BitmapImage(new Uri(sourcelogo)) };
            Children.Insert(0, _imagecategory);
            Children.Insert(1, _textblock);
            Children.Insert(2, logo);

        }
    }
}
