using System.Windows.Input;
using Storm.Mvvm.Commands;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class AddCollectionViewModel : AbstractViewModel
    {
        public ICommand WatchIndiagramCommand{ get; private set; }

        public AddCollectionViewModel()
        {
            WatchIndiagramCommand = new DelegateCommand(WatchIndiagram);
        }
        private void WatchIndiagram()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCH);
        }
    }
}
