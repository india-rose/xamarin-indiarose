using IndiaRose.Core.Interfaces;
using IndiaRose.Core.Services;
using Splat;

namespace IndiaRose.Core
{
	public static class Bootstrap
	{
		public static void Initialize()
		{
			Locator.CurrentMutable.RegisterLazySingleton(() => new LoggerService(), typeof(ILoggerService));
			Locator.CurrentMutable.RegisterLazySingleton(() => new SettingsService(), typeof(ISettingsService));
		}
	}
}
