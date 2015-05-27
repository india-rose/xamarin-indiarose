using Windows.UI;

namespace IndiaRose.Framework.Helper
{
	public static class ColorHelper
	{
		public static Color ToColor(this uint color)
		{
			byte a = (byte)(color >> 24);
			byte r = (byte)(color >> 16);
			byte g = (byte)(color >> 8);
			byte b = (byte)(color >> 0);
			return Color.FromArgb(a, r, g, b);
		}

		public static uint ToUint(this Color color)
		{
			return (uint)((color.A << 24) | (color.R << 16) |
						  (color.G << 8) | (color.B << 0));
		}
	}
}
