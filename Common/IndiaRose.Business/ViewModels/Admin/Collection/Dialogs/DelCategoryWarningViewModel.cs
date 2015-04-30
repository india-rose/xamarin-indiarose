

using System.Windows.Input;
using IndiaRose.Data.UIModel;
using IndiaRose.Storage;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class DelCategoryWarningViewModel : AbstractViewModel
    {
        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }
        private IndiagramContainer _indiagramContainer;

        [NavigationParameter]
        public IndiagramContainer IndiagramContainer
        {
            get { return _indiagramContainer; }
            set { SetProperty(ref _indiagramContainer, value); }
        }

        public ICommand DeleteCommand;

        public DelCategoryWarningViewModel()
        {
            DeleteCommand=new DelegateCommand(DeleteAction);
        }

        protected void DeleteAction()
        {
            CollectionStorageService.Delete(IndiagramContainer.Indiagram);
            BackAction();
        }
    }
}
