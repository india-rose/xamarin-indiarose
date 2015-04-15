using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using Android.Graphics.Drawables;
using IndiaRose.Data.UIModel;

namespace IndiaRose.Framework.Converters
{
    public class ColorContainerToDrawableColor : IValueConverter
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
		        return new ColorDrawable(Color.Transparent);
	        }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}