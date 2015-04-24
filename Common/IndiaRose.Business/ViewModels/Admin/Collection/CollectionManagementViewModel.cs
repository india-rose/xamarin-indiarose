using System.Windows.Input;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class CollectionManagementViewModel : AbstractViewModel
    {
	    public static ManagementViewModel SubViewModel;

        public ICommand AddCommand { get; private set; }
		public ICommand NextCommand { get; private set; }


        public CollectionManagementViewModel()
        {
            AddCommand = new DelegateCommand(AddAction);
			NextCommand = new DelegateCommand(NextAction);
        }

	    private void NextAction()
	    {
		    SubViewModel.NotifyNextAction();
	    }


        private void AddAction()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD);
        }
    }
}
