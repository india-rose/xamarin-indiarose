using System.Windows.Input;
using Storm.Mvvm.Commands;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class CollectionManagementViewModel : AbstractBrowserViewModel
    {
        public ICommand AddCommand { get; private set; }
		//public ICommand NextCommand { get; private set; }


        public CollectionManagementViewModel()
        {
            AddCommand = new DelegateCommand(AddAction);
			//NextCommand = new DelegateCommand(NextAction);
        }
		/*
	    private void NextAction()
	    {
			//TODO : implement
	    }
		*/

        private void AddAction()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD);
        }
    }
}
