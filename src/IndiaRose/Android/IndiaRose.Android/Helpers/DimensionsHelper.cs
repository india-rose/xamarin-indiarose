using Android.Content;
using Android.Util;

namespace IndiaRose.Droid.Helpers
{
	public static class DimensionsHelper
	{
		private static Context _context;

		public static void Initialize(Context context)
		{
			_context = context;
		}

		public static float DpToPixels(float dp) => dp * ((float)_context.Resources.DisplayMetrics.DensityDpi / (int)DisplayMetricsDensity.Default);

		public static float PixelsToDp(float pixels) => pixels / ((float)_context.Resources.DisplayMetrics.DensityDpi / (int)DisplayMetricsDensity.Default);
	}
}