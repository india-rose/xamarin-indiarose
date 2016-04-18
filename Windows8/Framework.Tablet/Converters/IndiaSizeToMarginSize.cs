using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IndiaRose.Framework.Converters
{
    public class IndiaSizeToMarginSize : IValueConverter
    {
        /// <summary>
        /// Donne la marge des Indiagrams à partir de la taille d'un Indiagram
        /// </summary>
        /// <param name="value">Taille d'un Indiagram</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="language">Inutile</param>
        /// <returns>Taille de la marge (Thickness)</returns>
        /// <see cref="Thickness"/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int x =(int) value;
            return new Thickness(((x*1.2) - x)/2);
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
