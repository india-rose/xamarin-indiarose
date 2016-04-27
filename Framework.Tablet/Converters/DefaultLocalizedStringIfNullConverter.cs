using System;
using Windows.UI.Xaml.Data;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Framework.Tablet.Converters
{
    /// <summary>
    /// Converter pour retourner une chaine par défaut si la resource voulu n'est pas présente
    /// </summary>
    public class DefaultLocalizedStringIfNullConverter : IValueConverter
    {
        /// <summary>
        /// Id de la chaine voulu
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// Property de la chaine voulu
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Renvoie une chaine par défaut si la resource n'est pas présente
        /// </summary>
        /// <param name="value">La chaine par défaut</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>La chaine voulu ou celle par défaut</returns>
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            string input = value as string;

            if (input == null)
            {
                if (Property == null)
                {
                    return LazyResolver<ILocalizationService>.Service.GetString(Uid);
                }

                return LazyResolver<ILocalizationService>.Service.GetString(Uid, Property);
            }
            return input;
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
