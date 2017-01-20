using Android.Content;
using IndiaRose.Core.Interfaces;

namespace IndiaRose.Droid.Services
{
	public class DeviceInfoService : IDeviceInfoService
	{
		private readonly Context _context;

		public DeviceInfoService(Context context)
		{
			_context = context;
		}

		public int Width => _context.Resources.DisplayMetrics.WidthPixels;
		public int Height => _context.Resources.DisplayMetrics.HeightPixels;
	}
}