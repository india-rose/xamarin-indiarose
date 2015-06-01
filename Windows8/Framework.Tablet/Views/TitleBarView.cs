using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;

namespace IndiaRose.Framework.Views
{
    public class TitleBarView : StackPanel
    {
	    private Image _logo;
	    private readonly Image _imagecategory;
	    private TextBlock _textblock;
	    private Category _category;

		public TitleBarView()
		{
			Orientation = Orientation.Vertical;
			var sourcelogo = "ms-appx:///Assets/" + "logoIndiaRose.png";
			_logo.Source= new BitmapImage(new Uri(sourcelogo));
			_imagecategory.Source = new BitmapImage(new Uri(_category.ImagePath));
			_imagecategory = new Image();
			_textblock.Text = _category.Text;
			Children.Add(_imagecategory);
			Children.Add(_textblock);
			Children.Add(_logo);
		}
    }
}
