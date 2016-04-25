using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IndiaRose.Framework.Converters
{
    public class ColorStringToDrawableColorConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une couleur sous forme de string en  ColorDrawable
        /// </summary>
        /// <param name="value">La chaine représentant la couleur</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>L'objet ColorDrawable de la couleur</returns>
        ///  <see cref="Color.ParseColor"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorString = value as string;

	        try
	        {
				Color colorResult = Color.ParseColor(colorString);
		        return new ColorDrawable(colorResult);
	        }
	        catch (Exception)
	        {
		        return new ColorDrawable(Color.Transparent);
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