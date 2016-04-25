#region Usings

using IndiaRose.Business.ViewModels.Admin;
using IndiaRose.Business.ViewModels.Admin.Collection;
using IndiaRose.Business.ViewModels.Admin.Collection.Dialogs;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
using IndiaRose.Business.ViewModels.User;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business
{
    /// <summary>
    /// Conteneur des ViewModels
    /// Sert pour les parties platform-dependant
    /// </summary>
	public class ViewModelsLocator
	{
		private static IContainer _container;

		public static void Initialize(IContainer container)
		{
			// Admin
			container.RegisterFactory(x => new HomeViewModel());
			container.RegisterFactory(x => new InstallVoiceSynthesisViewModel());
			container.RegisterFactory(x => new CreditsViewModel());
            container.RegisterFactory(x => new ServerSynchronizationViewModel());
            container.RegisterFactory(x => new MailErrorViewModel());

			// Admin/Settings
			container.RegisterFactory(x => new SettingsListViewModel());
			container.RegisterFactory(x => new ApplicationLookViewModel());
			container.RegisterFactory(x => new IndiagramPropertyViewModel());
            container.RegisterFactory(x => new AppBehaviourViewModel());

			// Admin/Settings/Dialogs
			container.RegisterFactory(x => new ColorPickerViewModel());
            container.RegisterFactory(x => new ResetSettingsViewModel());

            //Admin/Collection
            container.RegisterFactory(x => new WatchIndiagramViewModel());
            container.RegisterFactory(x => new CollectionManagementViewModel());
            container.RegisterFactory(x => new AddIndiagramViewModel());

            //Admin/Collection/Dialogs
			container.RegisterFactory(x => new ChooseCategoryViewModel());
            container.RegisterFactory(x => new ExploreCollectionCategoryViewModel());
			container.RegisterFactory(x => new ExploreCollectionIndiagramViewModel());
            container.RegisterFactory(x => new ImageChoiceViewModel());
            container.RegisterFactory(x => new SoundChoiceViewModel());
            container.RegisterFactory(x => new RecordSoundViewModel());
            container.RegisterFactory(x => new SelectCategoryActionViewModel());
            container.RegisterFactory(x => new DeleteIndiagramWarningViewModel());
            container.RegisterFactory(x => new DeleteCategoryWarningViewModel());
            container.RegisterFactory(x => new DeleteCategoryConfirmationViewModel());

            //User
			container.RegisterFactory(x => new UserViewModel());
			
			_container = container;
		}

		#region ViewModels : /Admin

		public HomeViewModel AdminHomeViewModel => _container.Resolve<HomeViewModel>();

        public InstallVoiceSynthesisViewModel AdminInstallTextToSpeechViewModel => _container.Resolve<InstallVoiceSynthesisViewModel>();

        public CreditsViewModel AdminCreditsViewModel => _container.Resolve<CreditsViewModel>();

        public ServerSynchronizationViewModel AdminServerSynchronizationViewModel => _container.Resolve<ServerSynchronizationViewModel>();

        public MailErrorViewModel AdminMailErrorViewModel => _container.Resolve<MailErrorViewModel>();

        #endregion

		#region ViewModels : /Admin/Settings

		public SettingsListViewModel AdminSettingsAppSettingsViewModel => _container.Resolve<SettingsListViewModel>();

        public ApplicationLookViewModel AdminSettingsApplicationLookViewModel => _container.Resolve<ApplicationLookViewModel>();

        public IndiagramPropertyViewModel AdminSettingsIndiagramPropertyViewModel => _container.Resolve<IndiagramPropertyViewModel>();

        public AppBehaviourViewModel AdminSettingsAppBehaviourViewModel => _container.Resolve<AppBehaviourViewModel>();

        #endregion


        #region ViewModels : /Admin/Settings/Dialogs

        public ColorPickerViewModel AdminSettingsDialogsColorPickerViewModel => _container.Resolve<ColorPickerViewModel>();

		
        public ResetSettingsViewModel AdminSettingsDialogsResetSettingsViewModel => _container.Resolve<ResetSettingsViewModel>();

        #endregion

        #region ViewModels : /Admin/Collection

        public CollectionManagementViewModel AdminCollectionManagementViewModel => _container.Resolve<CollectionManagementViewModel>();

        public AddIndiagramViewModel AdminCollectionAddIndiagramViewModel => _container.Resolve<AddIndiagramViewModel>();

        public WatchIndiagramViewModel AdminCollectionWatchIndiagramViewModel => _container.Resolve<WatchIndiagramViewModel>();

        #endregion

        #region ViewModels : /Admin/Collection/Dialogs

		public ChooseCategoryViewModel AdminCollectionDialogsChooseCategoryViewModel => _container.Resolve<ChooseCategoryViewModel>();

        public ExploreCollectionCategoryViewModel AdminCollectionDialogsExploreCollectionCategoryViewModel => _container.Resolve<ExploreCollectionCategoryViewModel>();

        public ExploreCollectionIndiagramViewModel AdminCollectionDialogsExploreCollectionIndiagramViewModel => _container.Resolve<ExploreCollectionIndiagramViewModel>();

        public ImageChoiceViewModel AdminCollectionDialogsImageChoiceViewModel => _container.Resolve<ImageChoiceViewModel>();

        public SoundChoiceViewModel AdminCollectionDialogsSoundChoiceViewModel => _container.Resolve<SoundChoiceViewModel>();

        public RecordSoundViewModel AdminCollectionDialogsRecordSoundViewModel => _container.Resolve<RecordSoundViewModel>();

        public SelectCategoryActionViewModel AdminCollectionDialogsSelectCategoryActionViewModel => _container.Resolve<SelectCategoryActionViewModel>();
        public DeleteIndiagramWarningViewModel AdminCollectionDialogsDeleteIndiagramWarningViewModel => _container.Resolve<DeleteIndiagramWarningViewModel>();
        public DeleteCategoryWarningViewModel AdminCollectionDialogsDeleteCategoryWarningViewModel => _container.Resolve<DeleteCategoryWarningViewModel>();

        public DeleteCategoryConfirmationViewModel AdminCollectionDialogsDeleteCategoryConfirmationViewModel => _container.Resolve<DeleteCategoryConfirmationViewModel>();

        #endregion

        #region ViewModels : /User
        public UserViewModel UserHomeViewModel => _container.Resolve<UserViewModel>();

        #endregion
    }
}