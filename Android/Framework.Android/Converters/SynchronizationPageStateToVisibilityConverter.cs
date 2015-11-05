using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using Android.Views;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Framework.Converters
{
	public class SynchronizationPageStateToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            LazyResolver<ILoggerService>.Service.Log(string.Format("Converter try value {0} compare to {1}", value, parameter), MessageSeverity.Info);

			string param = parameter as string;

			SynchronizationPageState paramState;
			if (!Enum.TryParse(param, out paramState))
			{
				throw new InvalidEnumArgumentException(string.Format("parameter is not a valid SynchronizationPageState enum member ({0})", parameter));
			}

			try
			{
				SynchronizationPageState state = (SynchronizationPageState) value;

                LazyResolver<ILoggerService>.Service.Log(string.Format("Converter result = {0}", state == paramState), MessageSeverity.Info);
                return (state == paramState) ? ViewStates.Visible : ViewStates.Gone;
			}
			catch (InvalidCastException)
			{
				throw new InvalidEnumArgumentException(string.Format("value is not a valid SynchronizationPageState enum member ({0}) of type {1}", value, value != null ? value.GetType().FullName : "<null>"));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}