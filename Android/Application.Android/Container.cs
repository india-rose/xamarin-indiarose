using System;
using System.Collections.Generic;
using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Services.Android;
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

            RegisterInstance<IResourceService>(new ResourceService(ActivityService));
			RegisterInstance<IEmailService>(new EmailService(this));
			RegisterInstance<IInstallVoiceSynthesisService>(new InstallVoiceSynthesisService(ActivityService));
			RegisterInstance<IScreenService>(new ScreenService(ActivityService));
			RegisterInstance<ISettingsService>(new SettingsService(this));
			RegisterInstance<IFontService>(new FontService(ActivityService));
		}
	}
}
