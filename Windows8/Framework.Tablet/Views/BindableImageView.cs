using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace IndiaRose.Framework.Views
{
    public class BindableImageView : StackPanel
    {
        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
            "ImagePath", typeof (string), typeof (BindableImageView), new PropertyMetadata(default(string), Refresh));


        public string ImagePath
        {
            get { return (string) GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof (int), typeof (BindableImageView), new PropertyMetadata(default(int),Refresh));

        public int Size
        {
            get { return (int) GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }


        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageView = d as BindableImageView;
            if (imageView != null) imageView.Refresh();
        }

        private readonly Image _image;
        private readonly StackPanel _redRect;

        public BindableImageView()
        {
            _redRect = new StackPanel()
            {
                Background = new SolidColorBrush(Colors.Red)
            };
            _image=new Image();
        }

        private void Refresh()
        {
            Height = Size;
            Width = Size;
            _image.Height = Size;
            _image.Width = Size;
            _redRect.Height = Size;
            _redRect.Width = Size;

            if (ImagePath != null)
            {
                Children.Clear();
                _image.Source = new BitmapImage(new Uri(ImagePath, UriKind.Absolute));
                Children.Add(_image);
            }
            else
            {
                Children.Clear();
                Children.Add(_redRect);
            }
        }
    }
}
