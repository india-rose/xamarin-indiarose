using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using IndiaRose.Data.UIModel;

namespace IndiaRose.Framework.Converters
{
    public class ColorContainerToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        ColorContainer colorContainer = value as ColorContainer;

	        if (colorContainer == null)
	        {
		        return null;
	        }

	        string colorString = colorContainer.Color;

	        try
	        {
				return Color.ParseColor(colorString);
	        }
	        catch (Exception)
	        {
		        return Color.Transparent;
	        }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}