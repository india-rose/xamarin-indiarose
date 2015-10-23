using System;
using System.Globalization;
using System.Windows.Data;

namespace IndiaRose.Framework.Converters
{
    public class ColorStringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        string colorString = value as string;
            if (colorString != null && colorString.StartsWith("#"))
            {
                string hexCode = colorString.Substring(1);
                return uint.Parse(hexCode, NumberStyles.HexNumber);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint input = (uint) value;
            return string.Format("#{0:X8}", input);
        }
    }
}