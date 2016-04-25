using System;
using System.Globalization;
using System.Windows.Data;
using Android.Graphics;
using Android.Graphics.Drawables;
using IndiaRose.Data.UIModel;

namespace IndiaRose.Framework.Converters
{
    public class ColorContainerToDrawableColorConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un ColorContainer en ColorDrawable
        /// </summary>
        /// <param name="value">L'objet ColorConverter</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>L'objet ColorDrawable correspondant</returns>
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