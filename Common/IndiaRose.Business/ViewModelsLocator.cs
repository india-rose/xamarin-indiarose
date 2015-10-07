#region Usings

using IndiaRose.Business.ViewModels;
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

			// Admin/Settings/Dialogs
			container.RegisterFactory(x => new ColorPickerViewModel());
			container.RegisterFactory(x => new DragAndDropViewModel());
			container.RegisterFactory(x => new ReadingDelayViewModel());
			container.RegisterFactory(x => new CategoryReadingViewModel());
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

		public HomeViewModel AdminHomeViewModel
		{
			get { return _container.Resolve<HomeViewModel>(); }
		}

		public InstallVoiceSynthesisViewModel AdminInstallTextToSpeechViewModel
		{
			get { return _container.Resolve<InstallVoiceSynthesisViewModel>(); }
		}

		public CreditsViewModel AdminCreditsViewModel
		{
			get { return _container.Resolve<CreditsViewModel>(); }
		}

		public ServerSynchronizationViewModel AdminServerSynchronizationViewModel
		{
			get { return _container.Resolve<ServerSynchronizationViewModel>(); }
		}

	    public MailErrorViewModel AdminMailErrorViewModel
	    {
            get { return _container.Resolve<MailErrorViewModel>(); }
	    }

		#endregion

		#region ViewModels : /Admin/Settings

		public SettingsListViewModel AdminSettingsAppSettingsViewModel
		{
			get { return _container.Resolve<SettingsListViewModel>(); }
		}

		public ApplicationLookViewModel AdminSettingsApplicationLookViewModel
		{
			get { return _container.Resolve<ApplicationLookViewModel>(); }
		}

		public IndiagramPropertyViewModel AdminSettingsIndiagramPropertyViewModel
		{
			get { return _container.Resolve<IndiagramPropertyViewModel>(); }
		}

		#endregion

        #region ViewModels : /Admin/Settings/Dialogs

        public ColorPickerViewModel AdminSettingsDialogsColorPickerViewModel
		{
			get { return _container.Resolve<ColorPickerViewModel>(); }
		}

		public DragAndDropViewModel AdminSettingsDialogsDragAndDropViewModel
		{
			get { return _container.Resolve<DragAndDropViewModel>(); }
		}

		public ReadingDelayViewModel AdminSettingsDialogsReadingDelayViewModel
		{
			get { return _container.Resolve<ReadingDelayViewModel>(); }
		}
        public ResetSettingsViewModel AdminSettingsDialogsResetSettingsViewModel
        {
            get { return _container.Resolve<ResetSettingsViewModel>(); }
        }

		public CategoryReadingViewModel AdminSettingsDialogsCategoryReadingViewModel
		{
			get { return _container.Resolve<CategoryReadingViewModel>(); }
		}

		#endregion

        #region ViewModels : /Admin/Collection

        public CollectionManagementViewModel AdminCollectionManagementViewModel
        {
            get { return _container.Resolve<CollectionManagementViewModel>(); }
        }

	    public AddIndiagramViewModel AdminCollectionAddIndiagramViewModel
	    {
            get { return _container.Resolve<AddIndiagramViewModel>(); }
	    }

	    public WatchIndiagramViewModel AdminCollectionWatchIndiagramViewModel
	    {
            get { return _container.Resolve<WatchIndiagramViewModel>(); }
	    }

        #endregion

        #region ViewModels : /Admin/Collection/Dialogs

		public ChooseCategoryViewModel AdminCollectionDialogsChooseCategoryViewModel
		{
			get { return _container.Resolve<ChooseCategoryViewModel>(); }
		}

		public ExploreCollectionCategoryViewModel AdminCollectionDialogsExploreCollectionCategoryViewModel
	    {
            get { return _container.Resolve<ExploreCollectionCategoryViewModel>(); }
	    }

		public ExploreCollectionIndiagramViewModel AdminCollectionDialogsExploreCollectionIndiagramViewModel
		{
			get { return _container.Resolve<ExploreCollectionIndiagramViewModel>(); }
		}

	    public ImageChoiceViewModel AdminCollectionDialogsImageChoiceViewModel
	    {
            get { return _container.Resolve<ImageChoiceViewModel>(); }
	    }

        public SoundChoiceViewModel AdminCollectionDialogsSoundChoiceViewModel
        {
            get { return _container.Resolve<SoundChoiceViewModel>(); }
        }

        public RecordSoundViewModel AdminCollectionDialogsRecordSoundViewModel
        {
            get { return _container.Resolve<RecordSoundViewModel>(); }
        }

		public SelectCategoryActionViewModel AdminCollectionDialogsSelectCategoryActionViewModel
        {
            get { return _container.Resolve<SelectCategoryActionViewModel>(); }
        }
		public DeleteIndiagramWarningViewModel AdminCollectionDialogsDeleteIndiagramWarningViewModel
        {
            get { return _container.Resolve<DeleteIndiagramWarningViewModel>(); }
        }
		public DeleteCategoryWarningViewModel AdminCollectionDialogsDeleteCategoryWarningViewModel
        {
            get { return _container.Resolve<DeleteCategoryWarningViewModel>(); }
        }

		public DeleteCategoryConfirmationViewModel AdminCollectionDialogsDeleteCategoryConfirmationViewModel
		{
			get { return _container.Resolve<DeleteCategoryConfirmationViewModel>(); }
		}
        #endregion

        #region ViewModels : /User
        public UserViewModel UserHomeViewModel
        {
			get { return _container.Resolve<UserViewModel>(); }
        }
        #endregion
    }
}