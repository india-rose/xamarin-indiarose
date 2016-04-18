using System;
using System.Globalization;
using System.Windows.Data;

namespace IndiaRose.Framework.Converters
{
    /// <summary>
    /// Convertit une couleur sous forme hexad�cimal en string en un uint
    /// </summary>
    public class ColorStringToIntConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une string repr�sentant une couleur en hexad�cimal en uint
        /// </summary>
        /// <param name="value">La chaine � convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>L'entier repr�sentant la couleur</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
        /// Convertit un uint en une string repr�sentant une couleur sous forme hexad�cimal
        /// </summary>
        /// <param name="value">L'uint � convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>La chaine repr�sentant une couleur en hexad�cimal</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint input = (uint) value;
            return string.Format("#{0:X8}", input);
        }
    }
}