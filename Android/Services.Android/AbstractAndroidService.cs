using System.Threading;
using System.Threading.Tasks;
using Android.App;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;

namespace IndiaRose.Services.Android
{
	public abstract class AbstractAndroidService : AbstractService, IInitializable
	{
		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		protected Activity CurrentActivity
		{
			get { return ActivityService.CurrentActivity; }
		}

		private readonly Semaphore _semaphore = new Semaphore(0, 1);

		public async Task InitializeAsync()
		{
			await Task.Run(() =>
			{
				if (CurrentActivity == null)
				{
					ActivityService.ActivityChanged += OnActivityChanged;
					if (CurrentActivity == null)
					{
						_semaphore.WaitOne();
					}
					ActivityService.ActivityChanged -= OnActivityChanged;
				}
			});

			await InitializeServiceAsync();
		}

		protected virtual async Task InitializeServiceAsync()
		{
			
		}

		private void OnActivityChanged(object sender, ValueChangedEventArgs<Activity> valueChangedEventArgs)
		{
			if (valueChangedEventArgs.NewValue != null)
			{
				_semaphore.Release();
			}
		}
	}
}