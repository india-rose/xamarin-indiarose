using System.Collections.Generic;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class SelectCategoryViewModel : AbstractBrowserViewModel
	{
		public ISettingsService SettingsService
		{
			get { return LazyResolver<ISettingsService>.Service; }
		}

		private IndiagramContainer _currentIndiagram;
		private IndiagramContainer _addIndiagramContainer;
		[NavigationParameter]
		public IndiagramContainer addIndiagramContainer
		{
			get { return _addIndiagramContainer; }
			set { SetProperty(ref _addIndiagramContainer, value); }
		}
		public IndiagramContainer CurrentIndiagram
		{
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SELECT, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram",new IndiagramContainer(indiagram)},{"AddIndiagram",addIndiagramContainer}
             });
		}
	}
}
