using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IndiaRose.Framework.Converters
{
    public class IndiaSizeToMarginSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int x =(int) value;
            return (int) ((x*1.2) - x)/2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
