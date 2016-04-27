using System;
using Windows.UI.Xaml.Data;

namespace Framework.Tablet.Converters
{
    public class TopAreaSizeToBottomAreaSizeConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un pourcentage de taille de la partie haute vers le pourcentage de la partie basse (utilisateur)
        /// </summary>
        /// <param name="value">Le pourcentage à convertir (en entier de 0 à 100)</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="language">Inutile</param>
        /// <returns>100-%PartieHaute</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int x = (int) value;
            return 100 - x;
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
