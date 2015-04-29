using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
	public class DeleteIndiagramViewModel : AbstractSettingsViewModel
	{
		private IndiagramContainer _indiagram;

		[NavigationParameter]
		public IndiagramContainer Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

        public ICommand WatchIndiagramCommand{ get; private set; }

        public DeleteIndiagramViewModel()
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
