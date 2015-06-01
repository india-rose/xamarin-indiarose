using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace IndiaRose.Framework.Converters
{
    public class ColorStringToIntConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            string colorString = value as string;
            if (colorString != null && colorString.StartsWith("#"))
            {
                string hexCode = colorString.Substring(1);
                return uint.Parse(hexCode, NumberStyles.HexNumber);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            uint input = (uint)value;
            return string.Format("#{0:X8}", input);
        }

    }
}
