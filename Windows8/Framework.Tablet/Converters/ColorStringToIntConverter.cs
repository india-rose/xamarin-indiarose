using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace IndiaRose.Framework.Converters
{
    /// <summary>
    /// Convertit une couleur sous forme hexadécimal en string en un uint
    /// </summary>
    public class ColorStringToIntConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une string représentant une couleur en hexadécimal en uint
        /// </summary>
        /// <param name="value">La chaine à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>L'entier représentant la couleur</returns>
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

        /// <summary>
        /// Convertit un uint en une string représentant une couleur sous forme hexadécimal
        /// </summary>
        /// <param name="value">L'uint à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>La chaine représentant une couleur en hexadécimal</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            uint input = (uint)value;
            return string.Format("#{0:X8}", input);
        }

    }
}
