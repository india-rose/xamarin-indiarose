using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;

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

	    }
    }
}
