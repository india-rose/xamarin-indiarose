using System;
using Windows.UI.Xaml.Data;

namespace IndiaRose.Framework.Converters
{
    public class BooleanToColumnSpanConverter : IValueConverter
    {
        /// <summary>
        /// Si le 1er paramètre est est vrai retourne 1 sinon 2
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? 1 : 2;
        }

        /// <summary>
        /// Non implémenté
        /// Retourne l'exception NotImplementedException
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
