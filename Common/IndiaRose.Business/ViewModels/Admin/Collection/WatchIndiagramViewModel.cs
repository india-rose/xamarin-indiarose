using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class WatchIndiagramViewModel : AbstractSettingsViewModel
	{
		public ICommand EditCommand { get; private set; }

		[NavigationParameter]
		public Indiagram CurrentIndiagram { get; private set; }

		public WatchIndiagramViewModel()
		{
			EditCommand = new DelegateCommand(EditAction);
		}
		private void EditAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD, new Dictionary<string, object>()
             {
                 {"InitialIndiagram",CurrentIndiagram}
             });
		}
	}
}
