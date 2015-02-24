using IndiaRose.Interfaces;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;

namespace IndiaRose.Services.Android
{
	public class ScreenService : AbstractServiceWithActivity, IScreenService
	{
		public ScreenService(IActivityService activityService)
			: base(activityService)
		{
		}

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