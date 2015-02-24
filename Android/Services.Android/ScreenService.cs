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

		public int ScreenWidth
		{
			get { return CurrentActivity.Window.DecorView.Width; }
		}

		public int ScreenHeight
		{
			get { return CurrentActivity.Window.DecorView.Height; }
		}
	}
}