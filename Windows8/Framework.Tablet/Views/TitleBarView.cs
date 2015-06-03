//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Foundation;
//using Windows.Security.ExchangeActiveSyncProvisioning;
//using Windows.UI;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Media.Imaging;
//using Windows.UI.Xaml.Shapes;
//using IndiaRose.Data.Model;

//namespace IndiaRose.Framework.Views
//{
//	public class TitleBarView : StackPanel
//	{
//		private Image _logo;
//		private  Image _imagecategory;
//		private TextBlock _textblock;

//		public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(
//			"Category", typeof (Category), typeof (TitleBarView), new PropertyMetadata(default(Category)));

//		public Category Category
//		{
//			get { return (Category) GetValue(CategoryProperty); }
//			set { SetValue(CategoryProperty, value); }
//		}

//		public TitleBarView()
//		{
//			Orientation = Orientation.Horizontal;
//			var sourcelogo = "ms-appx:///Assets/" + "logoIndiaRose.png";
//			_logo.Source = new BitmapImage(new Uri(sourcelogo, UriKind.RelativeOrAbsolute));
//			if (Category == null)
//			{
//				_textblock = new TextBlock { Text = "j'aime la salade"};
//			} 

//			if (Category == null)
//			{
//				_textblock = new TextBlock { Text = " " };
//				_imagecategory.Source = new BitmapImage(new Uri(sourcelogo));
//			}
//			else
//			{
//				var sourceimage = new Uri(Category.ImagePath, UriKind.Absolute);
//				_imagecategory.Source = new BitmapImage(sourceimage);
//				_textblock = new TextBlock { Text = Category.Text };
//			}
//			Children.Add(_imagecategory);
//			Children.Add(_textblock);
//			Children.Add(_logo);
//		}
//	}
//}
