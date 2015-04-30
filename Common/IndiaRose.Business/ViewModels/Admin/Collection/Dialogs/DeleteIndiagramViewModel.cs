using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
	public class DeleteIndiagramViewModel : AbstractSettingsViewModel
	{
		private IndiagramContainer _indiagram;

		[NavigationParameter]
		public IndiagramContainer Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

		public IMessageDialogService MessageDialogService
		{
			get { return LazyResolver<IMessageDialogService>.Service; }
		}

        public ICommand WatchIndiagramCommand{ get; private set; }
		public ICommand ConfirmeDeleteCommand { get; set; }

        public DeleteIndiagramViewModel()
        {
			ConfirmeDeleteCommand = new DelegateCommand(ConfirmeDeleteAction);
            WatchIndiagramCommand = new DelegateCommand(WatchIndiagram);
        }

		private void ConfirmeDeleteAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_CONFIRMATION, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram", Indiagram}
             });
		}
        private void WatchIndiagram()
        {
			NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCH, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram", Indiagram}
             });
        }
	}
}
