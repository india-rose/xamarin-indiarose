using System;
using System.Globalization;
using System.Windows.Data;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Framework.Converters
{
	public class DefaultLocalizedStringIfNullConverter : IValueConverter
	{
		public string Uid { get; set; }

		public string Property { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
