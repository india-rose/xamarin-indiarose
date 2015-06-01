using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IndiaRose.Framework.Converters
{
    public class IndiaSizeToReinforcerSizeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value*1.2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
