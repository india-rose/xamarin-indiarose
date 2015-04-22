using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;

namespace IndiaRose.Framework.Converters
{
    public class ColorStringToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorString = value as string;

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