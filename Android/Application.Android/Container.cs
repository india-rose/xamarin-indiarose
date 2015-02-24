using System.Security.AccessControl;
using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Services.Android;
using Storm.Mvvm.Inject;
using EmailService = IndiaRose.Services.EmailService;

namespace IndiaRose.Application
{
	public class Container : AndroidContainer
	{
        public readonly static ViewModelsLocator Locator = new ViewModelsLocator();

		protected override void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);

            ResourcesService resourcesService = new ResourcesService(ActivityService);
            RegisterInstance<IResourcesService>(resourcesService);

		    EmailService emailService = new EmailService();
            RegisterInstance<IEmailService>(emailService);

            InstallTTSService ttsService = new InstallTTSService(ActivityService);
            RegisterInstance<IInstallTTSService>(ttsService);

            ScreenService screenService = new ScreenService(ActivityService);
            RegisterInstance<IScreenService>(screenService);

            SettingsService settingsService = new SettingsService(ActivityService);
            RegisterInstance<ISettingsService>(settingsService);

            FontService fontService = new FontService(ActivityService);
            RegisterInstance<IFontService>(fontService);
		}
	}
}
