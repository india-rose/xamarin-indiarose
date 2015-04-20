using Android.App;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;

namespace IndiaRose.Services.Android
{
	public abstract class AbstractAndroidService : AbstractService
	{
		private IActivityService _activityService;

		protected IActivityService ActivityService
		{
			get { return _activityService ?? (_activityService = Container.Resolve<IActivityService>()); }
		}

		protected Activity CurrentActivity
		{
			get { return _activityService.CurrentActivity; }
		}

		protected AbstractAndroidService(IContainer container) : base(container)
		{
		}
	}
}