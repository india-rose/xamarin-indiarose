using System;
using System.Collections.Generic;
using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Services.Android;
using IndiaRose.Services.Android.Interfaces;
using IndiaRose.Storage.Sqlite;
using SQLite.Net.Platform.XamarinAndroid;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using Environment = Android.OS.Environment;
using TextToSpeechService = IndiaRose.Services.Android.TextToSpeechService;

namespace IndiaRose.Application
{
	public class Container : AndroidContainer
	{
        public readonly static ViewModelsLocator Locator = new ViewModelsLocator();

		private StorageService _storageService;
		private ISettingsService _settingsService;
		private ICollectionStorageService _collectionStorageService;
		private IInitializationStateService _initializationStateService;

		protected override void Initialize(Android.App.Application application, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			base.Initialize(application, views, dialogs);

			ILocalizationService localizationService = new LocalizationService(application, typeof(Resource.String));
			RegisterInstance(localizationService);
		}

		protected override void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);

			_storageService = new StorageService(Environment.ExternalStorageDirectory.Path);
			_settingsService = new SettingsService();
			_collectionStorageService = new SqliteCollectionStorageService(new SQLitePlatformAndroid());
			_initializationStateService = new InitializationStateService();

			RegisterInstance<IInitializationStateService>(_initializationStateService);

            RegisterInstance<IResourceService>(new ResourceService());
			RegisterInstance<IEmailService>(new EmailService());
			RegisterInstance<IInstallVoiceSynthesisService>(new InstallVoiceSynthesisService());
			RegisterInstance<IScreenService>(new ScreenService());
			RegisterInstance<IFontService>(new FontService());
            RegisterInstance<IMediaService>(new MediaService());
            RegisterInstance<IPopupService>(new PopupService());
            RegisterInstance<ICopyPasteService>(new CopyPasteService());

			TextToSpeechService textToSpeechService = new TextToSpeechService();
			textToSpeechService.Initialized += OnTtsInitialized;

            RegisterInstance<ITextToSpeechService>(textToSpeechService);

			RegisterInstance<IStorageService>(_storageService);
            RegisterInstance<ISettingsService>(_settingsService);
			RegisterInstance<IXmlService>(new XmlService());
			RegisterInstance<ICollectionStorageService>(_collectionStorageService);

			InitializeAsync();
		}

		private readonly object _mutex = new object();
		private bool _initializationFinished;

		private void OnTtsInitialized(object sender, EventArgs eventArgs)
		{
			lock (_mutex)
			{
				if (_initializationFinished)
				{
					_initializationStateService.InitializationFinished();
				}
				_initializationFinished = true;
			}
		}

		protected async void InitializeAsync()
		{
			await new ServiceInitializerWrapper(_storageService).InitializeAsync();
			await _settingsService.LoadAsync();
			await _collectionStorageService.InitializeAsync();

			lock (_mutex)
			{
				if (_initializationFinished)
				{
					_initializationStateService.InitializationFinished();
				}
				_initializationFinished = true;
			}
		}
	}
}
