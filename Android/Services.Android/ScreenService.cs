using IndiaRose.Interfaces;

namespace IndiaRose.Services.Android
{
	public class ScreenService : AbstractAndroidService, IScreenService
	{
		public int Width
		{
			get { return CurrentActivity.Window.DecorView.Width; }
		}

		public int Height
		{
			get { return CurrentActivity.Window.DecorView.Height; }
		}
	}
}