using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace IndiaRose.Framework.Converters
{
    public class ColorStringToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string color = value as string;
            if(color!=null && color.StartsWith("#"))
            {
                string hexCode = color.Substring(1);
                string opacity = hexCode.Substring(0, 2);
                string r = hexCode.Substring(2, 2);
                string v = hexCode.Substring(4, 2);
                string b = hexCode.Substring(6, 2);
                return new SolidColorBrush(Color.FromArgb(byte.Parse(opacity, NumberStyles.HexNumber), byte.Parse(r, NumberStyles.HexNumber), byte.Parse(v, NumberStyles.HexNumber), byte.Parse(b, NumberStyles.HexNumber)));
   }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var t = value as SolidColorBrush;
            if (t != null)
            {
                return t.Color.ToString();
            }
            return 0;
        }
    }
}
