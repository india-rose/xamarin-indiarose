using System.Threading;
using System.Threading.Tasks;
using Android.App;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;

namespace IndiaRose.Services.Android
{
	public class ServiceInitializerWrapper : IInitializable
	{
		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		private readonly Semaphore _semaphore = new Semaphore(0, 1);
		private readonly IInitializable _service;

		public ServiceInitializerWrapper(IInitializable service)
		{
			_service = service;
		}

		public async Task InitializeAsync()
		{
			await Task.Run(() =>
			{
				if (ActivityService.CurrentActivity == null)
				{
					ActivityService.ActivityChanged += OnActivityChanged;
					if (ActivityService.CurrentActivity == null)
					{
						_semaphore.WaitOne();
					}
					ActivityService.ActivityChanged -= OnActivityChanged;
				}
			});

			await _service.InitializeAsync();
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