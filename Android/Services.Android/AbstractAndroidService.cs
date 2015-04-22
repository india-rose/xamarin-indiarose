using Android.App;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;

namespace IndiaRose.Services.Android
{
	public abstract class AbstractAndroidService : AbstractService
	{
		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		protected Activity CurrentActivity
		{
			get { return ActivityService.CurrentActivity; }
		}
	}
}