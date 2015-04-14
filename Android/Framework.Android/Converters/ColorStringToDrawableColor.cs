using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IndiaRose.Framework.Converters
{
    public class ColorStringToDrawableColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorString = value as string;

	        try
	        {
				Color colorResult = Color.ParseColor(colorString);
		        return new ColorDrawable(colorResult);
	        }
	        catch (Exception)
	        {
		        return null;
	        }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}