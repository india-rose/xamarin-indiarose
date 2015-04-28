using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class CollectionManagementViewModel : AbstractBrowserViewModel
    {
        public ICommand AddCommand { get; private set; }

        public CollectionManagementViewModel()
        {
            AddCommand = new DelegateCommand(AddAction);
        }
		

        private void AddAction()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD);
        }

	    protected override void IndiagramSelectedAction(Indiagram indiagram)
	    {
		    base.IndiagramSelectedAction(indiagram);
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION, new Dictionary<string, object>()
             {
                 {"Indiagram",indiagram}
             });
	    }
    }
}
