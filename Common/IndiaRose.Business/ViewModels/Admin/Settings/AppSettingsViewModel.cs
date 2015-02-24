#region Usings

using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
	public class AppSettingsViewModel : AbstractBackViewModel
	{
		public ICommand BackgroundColorCommand { get; private set; }
		public ICommand ReadingDelayCommand { get; private set; }
		public ICommand DragAndDropCommand { get; private set; }
		public ICommand CategoryReadingCommand { get; private set; }
        public ICommand ResetSettingsCommand { get; private set; }
		public ICommand IndiagramPropertyCommand { get; private set; }

		public AppSettingsViewModel(IContainer container) : base(container)
		{
			BackgroundColorCommand = new DelegateCommand(BackgroundColorAction);
			ReadingDelayCommand = new DelegateCommand(ReadingDelayAction);
			DragAndDropCommand = new DelegateCommand(DragAndDropAction);
			CategoryReadingCommand = new DelegateCommand(CategoryReadingAction);
            ResetSettingsCommand = new DelegateCommand(ResetSettingsAction);
			IndiagramPropertyCommand = new DelegateCommand(IndiagramPropertyAction);
		}

		private void IndiagramPropertyAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_INDIAGRAMPROPERTIES);
		}

		private void BackgroundColorAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_BACKGROUNDCOLOR);
		}

		private void ReadingDelayAction()
		{
			IMessageDialogService messageDialogService = Container.Resolve<IMessageDialogService>();
			messageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_READINGDELAY);
		}

		private void DragAndDropAction()
		{
			IMessageDialogService messageDialogService = Container.Resolve<IMessageDialogService>();
			messageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_DRAGANDDROP);
		}

		private void CategoryReadingAction()
		{
			IMessageDialogService messageDialogService = Container.Resolve<IMessageDialogService>();
			messageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_CATEGORYREADING);
		}
        private void ResetSettingsAction()
        {
            IMessageDialogService messageDialogService = Container.Resolve<IMessageDialogService>();
            messageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_RESETSETTINGS);
        }
	}
}