using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Storage;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class DeleteWarningViewModel : AbstractViewModel
    {
        #region Command
        public ICommand DeleteCommand { get; private set; }
        #endregion
        #region Properties
        private IndiagramContainer _indiagramContainer;
        
        [NavigationParameter]
        public IndiagramContainer IndiagramContainer
        {
            get { return _indiagramContainer; }
            set { SetProperty(ref _indiagramContainer, value); }
        }
        #endregion
        #region Service
        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }
        #endregion
        public DeleteWarningViewModel()
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
