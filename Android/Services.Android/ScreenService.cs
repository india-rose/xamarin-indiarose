using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Services.Android
{
	public class ScreenService : AbstractAndroidService, IScreenService
	{
		public ScreenService(IContainer container)
			: base(container)
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