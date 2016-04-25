using System;
using System.Globalization;
using System.Windows.Data;
using IndiaRose.Data.UIModel;

namespace IndiaRose.Framework.Converters
{
    public class ColorContainerToIntConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un ColorContainer en uint
        /// </summary>
        /// <param name="value">L'objet ColorConverter</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>L'entier correspondant</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        ColorContainer colorContainer = value as ColorContainer;

	        if (colorContainer == null)
	        {
		        return null;
	        }

	        string colorString = colorContainer.Color;
            if (colorString.StartsWith("#"))
            {
                string hexCode = colorString.Substring(1);
                return uint.Parse(hexCode, NumberStyles.HexNumber);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint input = (uint) value;
            return new ColorContainer
            {
                Color = $"#{input:X8}"
            };
        }
    }
}