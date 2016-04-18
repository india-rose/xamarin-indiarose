using System.Threading.Tasks;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels
{
    /// <summary>
    /// VueModèle de la page de chargement
    /// </summary>
	public class SplashScreenViewModel : AbstractViewModel
	{
		private const string PROPERTY = "Text";
		private const string LOADING_UID = "SplashScreen_Loading";
		private const string FROM_OLD_FORMAT_UID = "SplashScreen_FromOldFormat";
		private const string FROM_ZIP_UID = "SplashScreen_FromZip";

		public enum LaunchingType
		{
			User,
			Admin
		}

		private readonly LaunchingType _launchingType;
		private string _status;

		public string Status
		{
			get { return _status; }
			set { SetProperty(ref _status, value); }
		}

		public SplashScreenViewModel(LaunchingType launchingType)
		{
			_launchingType = launchingType;

			Status = LazyResolver<ILocalizationService>.Service.GetString(LOADING_UID, PROPERTY);
			LazyResolver<IInitializationStateService>.Service.AddInitializedCallback(OnInitialized);
		}

        /// <summary>
        /// Check s'il y a besoin d'importer la collection
        /// Si oui l'importe depuis le bon format puis va dans la bonne partie de l'application (Admin ou User)
        /// Sinon va juste dans la bonne partie de l'application
        /// </summary>
		private void OnInitialized()
		{
			Task.Run(async () =>
			{
				ICollectionStorageService collectionStorageService = LazyResolver<ICollectionStorageService>.Service;
				IXmlService xmlService = LazyResolver<IXmlService>.Service;
				IResourceService resourceService = LazyResolver<IResourceService>.Service;

				if (collectionStorageService.Collection.Count == 0)
				{
					if (await xmlService.HasOldCollectionFormatAsync())
					{
						DispatcherService.InvokeOnUIThread(() => Status = LocalizationService.GetString(FROM_OLD_FORMAT_UID, PROPERTY));
						await xmlService.InitializeCollectionFromOldFormatAsync();
					}
					else
					{
						DispatcherService.InvokeOnUIThread(() => Status = LocalizationService.GetString(FROM_ZIP_UID, PROPERTY));
						await xmlService.InitializeCollectionFromZipStreamAsync(await resourceService.OpenZip("indiagrams.zip"));
					}
				}

				DispatcherService.InvokeOnUIThread(() => NavigationService.Navigate(_launchingType == LaunchingType.User ? Views.USER_HOME : Views.ADMIN_HOME));
			});
		}
	}
}
