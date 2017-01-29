using Android.Content;
using IndiaRose.Core.Interfaces;
using IndiaRose.Droid.Helpers;
using IndiaRose.Droid.Services;
using Splat;

namespace IndiaRose.Droid
{
	public static class Bootstrap
	{
		private static readonly object _mutex = new object();
		private static bool _initialized;

		public static void Initialize(Context applicationContext)
		{
			if (_initialized)
			{
				return;
			}

			lock (_mutex)
			{
				if (_initialized)
				{
					return;
				}
				_initialized = true;
			}

			LocalizedStrings.Initialize(applicationContext);
			DimensionsHelper.Initialize(applicationContext);

			Core.Bootstrap.Initialize();
			Core.Admins.Bootstrap.Initialize();
			Core.Users.Bootstrap.Initialize();

			Locator.CurrentMutable.RegisterLazySingleton(() => new DeviceInfoService(applicationContext), typeof(IDeviceInfoService));
			Locator.CurrentMutable.RegisterLazySingleton(() => new FontService(), typeof(IFontService));
		}
	}
}
