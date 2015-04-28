using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class AddCollectionViewModel : AbstractViewModel
    {
		[NavigationParameter]
		public Indiagram Indiagram { get; set; }

        public ICommand WatchIndiagramCommand{ get; private set; }

        public AddCollectionViewModel()
        {
            WatchIndiagramCommand = new DelegateCommand(WatchIndiagram);
        }
        private void WatchIndiagram()
        {
			NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCH, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram",Indiagram}
             });
        }
    }
}
