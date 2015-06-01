#region Usings

using System.Windows.Input;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
	public class SettingsListViewModel : AbstractViewModel
	{
		#region Commands

		public ICommand ApplicationLookCommand { get; private set; }
		public ICommand IndiagramPropertyCommand { get; private set; }
		public ICommand DragAndDropCommand { get; private set; }

		public ICommand ReadingDelayCommand { get; private set; }
		public ICommand CategoryReadingCommand { get; private set; }
		public ICommand ResetSettingsCommand { get; private set; }

		#endregion

		public SettingsListViewModel()
		{
			ApplicationLookCommand = new DelegateCommand(ApplicationLookAction);
			ReadingDelayCommand = new DelegateCommand(ReadingDelayAction);
			DragAndDropCommand = new DelegateCommand(DragAndDropAction);
			CategoryReadingCommand = new DelegateCommand(CategoryReadingAction);
            ResetSettingsCommand = new DelegateCommand(ResetSettingsAction);
			IndiagramPropertyCommand = new DelegateCommand(IndiagramPropertyAction);
		}

		#region Commands implementation

		private void ApplicationLookAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_APPLICATIONLOOK);
		}

		private void IndiagramPropertyAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_INDIAGRAMPROPERTIES);
		}

		private void DragAndDropAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_DRAGANDDROP);
		}


		private void CategoryReadingAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_CATEGORYREADING);
		}

		private void ReadingDelayAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_READINGDELAY);
		}

		private void ResetSettingsAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_RESETSETTINGS);
		}

		#endregion
	}
}