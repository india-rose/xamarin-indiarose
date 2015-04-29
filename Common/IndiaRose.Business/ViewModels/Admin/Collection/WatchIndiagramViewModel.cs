using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class WatchIndiagramViewModel : AbstractSettingsViewModel
	{

        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }
		public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

		private IndiagramContainer _currentIndiagram;

		[NavigationParameter]
		public IndiagramContainer CurrentIndiagram
		{
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}

		public WatchIndiagramViewModel()
		{
			EditCommand = new DelegateCommand(EditAction);
            DeleteCommand=new DelegateCommand(DeleteAction);
		}
		private void EditAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD, new Dictionary<string, object>()
             {
                 {"InitialIndiagram",CurrentIndiagram}
             });
		}

	    protected void DeleteAction()
	    {
	        MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING, new Dictionary<string, object>()
	        {
	            {"IndiagramContainer",CurrentIndiagram}
	        });
	    }
	}
}
