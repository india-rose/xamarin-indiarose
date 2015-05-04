using System;
using System.Collections.Generic;
using Android.Speech.Tts;
using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Services.Android;
using IndiaRose.Storage;
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

		protected override void Initialize(Android.App.Application application, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			base.Initialize(application, views, dialogs);

			ILocalizationService localizationService = new LocalizationService(application, typeof(Resource.String));
			RegisterInstance(localizationService);
		}

		protected override async void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);

			IStorageService storageService = new StorageService(Environment.ExternalStorageDirectory.Path);
			await storageService.InitializeAsync();

			ISettingsService settingsService = new SettingsService();

            RegisterInstance<IResourceService>(new ResourceService());
			RegisterInstance<IEmailService>(new EmailService());
			RegisterInstance<IInstallVoiceSynthesisService>(new InstallVoiceSynthesisService());
			RegisterInstance<IScreenService>(new ScreenService());
			RegisterInstance<IFontService>(new FontService());
            RegisterInstance<IMediaService>(new MediaService());
            RegisterInstance<IPopupService>(new PopupService());
            RegisterInstance<ICopyPasteService>(new CopyPasteService());
            RegisterInstance<ITextToSpeechService>(new TextToSpeechService());

			RegisterInstance<IStorageService>(storageService);
			RegisterInstance<ICollectionStorageService>(new SqliteCollectionStorageService(new SQLitePlatformAndroid()));
            RegisterInstance<ISettingsService>(settingsService);
			await settingsService.LoadAsync();

		}
	}
}
