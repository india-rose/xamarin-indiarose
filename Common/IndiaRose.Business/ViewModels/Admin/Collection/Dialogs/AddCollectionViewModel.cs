using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storm.Mvvm.Commands;
using System.Windows.Input;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class AddCollectionViewModel : AbstractViewModel
    {
        public ICommand WatchIndiagramCommand{ get; private set; }

        public AddCollectionViewModel(IContainer container)
            : base(container)
        {
            WatchIndiagramCommand = new DelegateCommand(WatchIndiagram);
        }
        private void WatchIndiagram()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCH);
        }
    }
}
