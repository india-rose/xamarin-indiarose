using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using IndiaRose.Data.UIModel;

namespace IndiaRose.Framework.Converters
{
    public class ColorContainerToColorConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un ColorContainer en Color
        /// </summary>
        /// <param name="value">L'objet ColorConverter</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>L'objet Color correspondant</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        ColorContainer colorContainer = value as ColorContainer;

	        if (colorContainer == null)
	        {
		        return null;
	        }

	        string colorString = colorContainer.Color;

	        try
	        {
				return Color.ParseColor(colorString);
	        }
	        catch (Exception)
	        {
		        return Color.Transparent;
	        }
        }

        /// <summary>
        /// Non implémenté
        /// Retourne l'exception NotImplementedException
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}