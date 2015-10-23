using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
	public abstract class AbstractService
	{
		#region Service property

		protected ILocalizationService LocalizationService
		{
			get { return LazyResolver<ILocalizationService>.Service; }
		}

		protected ILoggerService LoggerService
		{
			get { return LazyResolver<ILoggerService>.Service; }
		}

		#endregion
	}
}
