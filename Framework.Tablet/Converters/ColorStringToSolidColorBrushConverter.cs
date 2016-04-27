using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Framework.Tablet.Converters
{
    public class ColorStringToSolidColorBrushConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une string en SolidColorBrush
        /// </summary>
        /// <param name="value">La chaine en format hexadécimal (argb)</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="language">Inutile</param>
        /// <returns>L'objet SolidColorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string color = value as string;
            if (color != null && color.StartsWith("#"))
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

        /// <summary>
        /// Convertit un SolidColorBrush vers un string représentant une couleur en hexadécimal argb
        /// </summary>
        /// <param name="value">Le SolidColorBrush à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="language">Inutile</param>
        /// <returns>La chaine représentant la couleur</returns>
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
