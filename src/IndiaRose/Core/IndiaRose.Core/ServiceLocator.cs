using System;
using IndiaRose.Core.Interfaces;
using Splat;

namespace IndiaRose.Core
{
	public static class ServiceLocator
	{
		private static readonly Lazy<ILoggerService> _loggerService = new Lazy<ILoggerService>(() => Locator.Current.GetService<ILoggerService>());
		private static readonly Lazy<ISettingsService> _settingsService = new Lazy<ISettingsService>(() => Locator.Current.GetService<ISettingsService>());
		private static readonly Lazy<IDeviceInfoService> _deviceInfoService= new Lazy<IDeviceInfoService>(() => Locator.Current.GetService<IDeviceInfoService>());
		private static readonly Lazy<IFontService> _fontService = new Lazy<IFontService>(() => Locator.Current.GetService<IFontService>());

		public static ILoggerService LoggerService => _loggerService.Value;
		public static ISettingsService SettingsService => _settingsService.Value;
		public static IDeviceInfoService DeviceInfoService => _deviceInfoService.Value;
		public static IFontService FontService => _fontService.Value;
	}
}
