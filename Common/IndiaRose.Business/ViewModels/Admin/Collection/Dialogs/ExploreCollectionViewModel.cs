using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ExploreCollectionViewModel : AbstractSettingsViewModel
    {
		private Indiagram _indiagram;

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

        public ICommand WatchIndiagramCommand{ get; private set; }

        public ExploreCollectionViewModel()
        {
            WatchIndiagramCommand = new DelegateCommand(WatchIndiagram);
        }
        private void WatchIndiagram()
        {
			NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCH, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram", Indiagram}
             });
        }
    }
}
