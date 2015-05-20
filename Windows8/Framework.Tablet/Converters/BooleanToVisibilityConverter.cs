using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace IndiaRose.Framework.Converters
{
    class BooleanToVisibilityConverter
    {
        public object Convert(object value)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
