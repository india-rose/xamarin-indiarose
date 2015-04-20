using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
	public abstract class AbstractService : NotifierBase
	{
		#region Service backing fields

		private ILocalizationService _localizationService;
		private ILoggerService _loggerService;

		#endregion

		protected IContainer Container { get; private set; }

		#region Service property

		protected ILocalizationService LocalizationService
		{
			get { return _localizationService ?? (_localizationService = Container.Resolve<ILocalizationService>()); }
		}

		protected ILoggerService LoggerService
		{
			get { return _loggerService ?? (_loggerService = Container.Resolve<ILoggerService>()); }
		}

		#endregion


		protected AbstractService(IContainer container)
		{
			Container = container;
		}
	}
}
