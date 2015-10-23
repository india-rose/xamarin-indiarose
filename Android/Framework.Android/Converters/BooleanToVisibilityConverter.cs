using System;
using System.Globalization;
using System.Windows.Data;
using Android.Views;

namespace IndiaRose.Framework.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter as string ?? "";
            bool res = (bool) value;
	        if (Equals(param, "Negation"))
	        {
		        res = !res;
	        }
	        return res ? ViewStates.Visible : ViewStates.Gone;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}