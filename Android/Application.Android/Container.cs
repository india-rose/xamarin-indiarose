using System;
using System.IO;
using System.Collections.Generic;
using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Services.Android;
using IndiaRose.Storage;
using IndiaRose.Storage.Sqlite;
using SQLite.Net.Platform.XamarinAndroid;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

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

		protected override void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);

        
            RegisterInstance<IResourceService>(new ResourceService());
			RegisterInstance<IEmailService>(new EmailService());
			RegisterInstance<IInstallVoiceSynthesisService>(new InstallVoiceSynthesisService());
			RegisterInstance<IScreenService>(new ScreenService());
			RegisterInstance<ISettingsService>(new SettingsService());
			RegisterInstance<IFontService>(new FontService());
			RegisterInstance<ICollectionStorageService>(new SqliteCollectionStorageService(new SQLitePlatformAndroid(), Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "IndiaRose")));
            RegisterInstance<IMediaService>(new MediaService());
            RegisterInstance<IPopupService>(new PopupService());

	    }
	}
}
