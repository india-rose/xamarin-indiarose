using System;
using Windows.UI.Xaml.Data;

namespace IndiaRose.Framework.Converters
{
    public class IndiaSizeToReinforcerSizeConverter : IValueConverter
    {

        /// <summary>
        /// Donne la taille du renforçateur des Indiagrams à partir de la taille d'un Indiagram
        /// </summary>
        /// <param name="value">Taille d'un Indiagram</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="language">Inutile</param>
        /// <returns>Taille du renforçateur (int)</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value*1.2;
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
