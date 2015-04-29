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
        public ICommand DeleteCommand { get; private set; }
        private IndiagramContainer _indiagramContainer;
        
        [NavigationParameter]
        public IndiagramContainer IndiagramContainer
        {
            get { return _indiagramContainer; }
            set { SetProperty(ref _indiagramContainer, value); }
        }

        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }

        public DeleteWarningViewModel()
        {
           DeleteCommand=new DelegateCommand(DeleteAction); 
        }

        protected void DeleteAction()
        {
            //todo voir avec julien pour fils;
           /* Category b=null;
            if (IndiagramContainer.Indiagram.IsCategory)
                b = IndiagramContainer.Indiagram as Category;
            foreach (var india in b.Children)
            {
                CollectionStorageService.Delete(india);
            }
            * */
            CollectionStorageService.Delete(IndiagramContainer.Indiagram);
            BackAction();
        }
    }
}
