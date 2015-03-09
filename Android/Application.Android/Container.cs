using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Services.Android;
using Storm.Mvvm.Inject;

namespace IndiaRose.Application
{
	public class Container : AndroidContainer
	{
        public readonly static ViewModelsLocator Locator = new ViewModelsLocator();

		protected override void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);

            ResourceService resourceService = new ResourceService(ActivityService);
            RegisterInstance<IResourceService>(resourceService);

		    EmailService emailService = new EmailService(this);
            RegisterInstance<IEmailService>(emailService);

            InstallVoiceSynthesisService voiceSynthesisService = new InstallVoiceSynthesisService(ActivityService);
            RegisterInstance<IInstallVoiceSynthesisService>(voiceSynthesisService);

            ScreenService screenService = new ScreenService(ActivityService);
            RegisterInstance<IScreenService>(screenService);

            SettingsService settingsService = new SettingsService(this);
            RegisterInstance<ISettingsService>(settingsService);

            FontService fontService = new FontService(ActivityService);
            RegisterInstance<IFontService>(fontService);
		}
	}
}
