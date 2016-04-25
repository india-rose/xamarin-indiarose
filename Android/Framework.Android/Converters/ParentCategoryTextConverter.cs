using System;
using System.Globalization;
using System.Windows.Data;
using IndiaRose.Data.Model;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Framework.Converters
{
    /// <summary>
    /// Converter pour passer d'un Indiagram en texte
    /// </summary>
	public class ParentCategoryTextConverter : IValueConverter
	{
		public string Uid { get; set; }

		public string Property { get; set; }

        /// <summary>
        /// Convertit un Indiagram en texte
        /// </summary>
        /// <param name="value">Indiagram à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="culture">Inutile</param>
        /// <returns>Retourne le texte de l'Indiagram</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
        /// Renvoie l'exception NotImplemetedException
        /// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
