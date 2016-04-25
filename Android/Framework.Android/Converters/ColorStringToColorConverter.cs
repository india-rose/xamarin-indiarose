using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;

namespace IndiaRose.Framework.Converters
{
    public class ColorStringToColorConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une couleur sous forme de string en  Color
        /// </summary>
        /// <param name="value">La chaine représentant la couleur</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>L'objet Color de la couleur</returns>
        ///  <see cref="Color.ParseColor"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorString = value as string;

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