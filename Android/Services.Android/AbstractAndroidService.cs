using System.Threading;
using System.Threading.Tasks;
using Android.App;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;

namespace IndiaRose.Services.Android
{
    /// <summary>
    /// Classe abstraite définissant quelques outils utiles pour les services Android
    /// </summary>
	public abstract class AbstractAndroidService : AbstractService, IInitializable
	{
        /// <summary>
        /// Service sur les Activités Android
        /// </summary>
		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

        /// <summary>
        /// Activité courante
        /// </summary>
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

        /// <summary>
        /// Initialise le service
        /// Méthode asynchrone
        /// </summary>
        /// <returns>La tâche asynchrone initialisant le service</returns>
		protected virtual async Task InitializeServiceAsync()
		{
			
		}

        /// <summary>
        /// Callback pour le changement d'activité
        /// </summary>
		private void OnActivityChanged(object sender, ValueChangedEventArgs<Activity> valueChangedEventArgs)
		{
			if (valueChangedEventArgs.NewValue != null)
			{
				_semaphore.Release();
			}
		}
	}
}