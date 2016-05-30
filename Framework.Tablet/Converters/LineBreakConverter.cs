using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Framework.Tablet.Converters
{
    public class LineBreakConverter : IValueConverter
    {
        /// <summary>
        /// Remplace, dans une string, toutes les occurences de "\n" par un CR (Carriage return, code ascii 13) suivi d'un LF (Line feed, code ascii 10)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string content = (string) value;
            content = content.Replace("\\n", System.Convert.ToChar(13) + "" + System.Convert.ToChar(10));
            return content;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string content = (string)value;
            content = content.Replace(System.Convert.ToChar(13) + "" + System.Convert.ToChar(10), "\\n");
            return content;
        }
    }
}
