using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IndiaRose.Framework.Converters
{
    public class IntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint color = (uint) value;

	        // ReSharper disable once RedundantCast
			// Mandatory cause of Android stupidity
            return (Drawable)(new ColorDrawable(new Color((int) color)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}