using System;
using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Messaging;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ExploreCollectionViewModel : AbstractSettingsViewModel
    {
		private IndiagramContainer _indiagram;

		[NavigationParameter]
		public IndiagramContainer Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}
		public ICommand GoInToCommand { get; set; }
        public ICommand WatchIndiagramCommand{ get; private set; }

        public ExploreCollectionViewModel()
        {
            WatchIndiagramCommand = new DelegateCommand(WatchIndiagram);
			GoInToCommand = new DelegateCommand(GoInToAction);
        }


		private void GoInToAction()
		{
			LazyResolver<IMessageDialogService>.Service.DismissCurrentDialog();
			Messenger.Send<Indiagram>(Messages.ADMIN_COLLECTION_GOINTO, Indiagram.Indiagram);
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
