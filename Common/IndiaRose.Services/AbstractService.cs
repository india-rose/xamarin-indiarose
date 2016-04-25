using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
	public abstract class AbstractService
	{
		#region Service property

		protected ILocalizationService LocalizationService => LazyResolver<ILocalizationService>.Service;

	    protected ILoggerService LoggerService => LazyResolver<ILoggerService>.Service;

	    #endregion
	}
}
