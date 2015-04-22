using System.Windows.Input;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class CollectionManagementViewModel : AbstractViewModel
    {
        public ICommand AddCommand { get; private set; }
        public ICommand AddCollection{ get; private set; }

        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }

        public CollectionManagementViewModel()
        {
            AddCommand = new DelegateCommand(AddAction);
            AddCollection = new DelegateCommand(AddCollectionAction);
        }

        private void AddCollectionAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_ADDCOLLECTION);
        }

        private void AddAction()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD);
        }
    }
}
