using IndiaRose.Interfaces;

namespace IndiaRose.Services.Android
{
	public class ScreenService : AbstractAndroidService, IScreenService
	{
		public int Width => CurrentActivity.Window.DecorView.Width;

	    public int Height => CurrentActivity.Window.DecorView.Height;
	}
}