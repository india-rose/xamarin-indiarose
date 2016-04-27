using System;
using Windows.UI.Xaml.Data;
using IndiaRose.Data.Model;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Framework.Tablet.Converters
{
    public class ParentCategoryTextConverter : IValueConverter
    {
        public string Uid { get; set; }

        public string Property { get; set; }

        /// <summary>
        /// Convertit un Indiagram en texte
        /// </summary>
        /// <param name="value">L'Indiagram à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>Le texte pour l'Indiagram donné</returns>
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            Indiagram input = value as Indiagram;

            if (input == null)
            {
                if (Property == null)
                {
                    return LazyResolver<ILocalizationService>.Service.GetString(Uid);
                }

                return LazyResolver<ILocalizationService>.Service.GetString(Uid, Property);
            }
            return input.Text;
        }

        /// <summary>
        /// Non implémenté
        /// Retourne l'exception NotImplementedException
        /// </summary
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
