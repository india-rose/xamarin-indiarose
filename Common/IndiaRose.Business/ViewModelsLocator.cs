#region Usings

using IndiaRose.Business.ViewModels.Admin;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
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
			container.RegisterFactory(x => new HomeViewModel(x));
			container.RegisterFactory(x => new InstallVoiceSynthesisViewModel(x));
			container.RegisterFactory(x => new CreditsViewModel(x));
			container.RegisterFactory(x => new ServerSynchronizationViewModel(x));

			// Admin/Settings
			container.RegisterFactory(x => new SettingsListViewModel(x));
			container.RegisterFactory(x => new ApplicationLookViewModel(x));
			container.RegisterFactory(x => new IndiagramPropertyViewModel(x));


			// Admin/Settings/Dialogs
			container.RegisterFactory(x => new ColorPickerViewModel(x));
			container.RegisterFactory(x => new DragAndDropViewModel(x));
			container.RegisterFactory(x => new ReadingDelayViewModel(x));
			container.RegisterFactory(x => new CategoryReadingViewModel(x));
            container.RegisterFactory(x => new ResetSettingsViewModel(x));

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

		#endregion

		#region ViewModels : /Admin/Settings

		public SettingsListViewModel AdminSettingsAppSettingsViewModel
		{
			get { return _container.Resolve<SettingsListViewModel>(); }
		}

		public ApplicationLookViewModel AdminSettingsBackgroundColorViewModel
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

        public CollectionManagementActivityViewModel AdminCollectionManagementActivityViewModel
	    {
            get { return _container.Resolve<ResetSettingsViewModel>(); }
	    }


		#endregion
	}
}