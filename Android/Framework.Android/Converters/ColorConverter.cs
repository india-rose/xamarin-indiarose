using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using Android.Graphics.Drawables;
using IndiaRose.Data.UIModel;

namespace IndiaRose.Framework.Converters
{
    public class ColorConverter : IValueConverter
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
				Color colorResult = Color.ParseColor(colorString);
		        return new ColorDrawable(colorResult);
	        }
	        catch (Exception)
	        {
		        return null;
	        }


	        // ReSharper disable once RedundantCast
			// Mandatory cause of Android stupidity
            //return (Drawable)(new ColorDrawable(new Color((int) color)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}