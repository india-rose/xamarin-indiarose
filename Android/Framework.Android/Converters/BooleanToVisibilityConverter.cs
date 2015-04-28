using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Android.App;
using Android.Content;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Visibility = Android.Transitions.Visibility;

namespace IndiaRose.Framework.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception(value.ToString() + "\n" + targetType.ToString() + "\n" + parameter.ToString() + "\n" + culture.ToString());
            /*bool b = value as bool;
            if (b != null)
            {
                return b ? ViewStates.Visible : ViewStates.Gone;
            }
             */
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}