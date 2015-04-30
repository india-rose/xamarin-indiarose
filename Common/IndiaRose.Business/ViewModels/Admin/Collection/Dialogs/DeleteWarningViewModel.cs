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
using Storm.Mvvm.Services;

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
        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }
        #endregion
        public DeleteWarningViewModel()
        {
            DeleteCommand = new DelegateCommand(DeleteAction);
        }

        protected void DeleteAction()
        {
            if (IndiagramContainer.Indiagram.HasChildren())
            {
                    MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELCATEGORYWARNING, new Dictionary<string, object>
	                {
	                    {"IndiagramContainer",IndiagramContainer}
	                });
                    MessageDialogService.DismissCurrentDialog();
            }
            else
            {
                CollectionStorageService.Delete(IndiagramContainer.Indiagram);
                BackAction();
            }
        }
    }
}
