using System;
using System.Collections.Generic;
using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Services.Android;
using IndiaRose.Storage;
using IndiaRose.Storage.Sqlite;
using SQLite.Net.Platform.XamarinAndroid;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
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

            RegisterInstance<IActivityService>(ActivityService);
            RegisterInstance<IResourceService>(new ResourceService(this));
			RegisterInstance<IEmailService>(new EmailService(this));
			RegisterInstance<IInstallVoiceSynthesisService>(new InstallVoiceSynthesisService(this));
			RegisterInstance<IScreenService>(new ScreenService(this));
			RegisterInstance<ISettingsService>(new SettingsService(this));
			RegisterInstance<IFontService>(new FontService(this));
			RegisterInstance<ICollectionStorageService>(new SqliteCollectionStorageService(new SQLitePlatformAndroid()));
			
		}
	}
}
