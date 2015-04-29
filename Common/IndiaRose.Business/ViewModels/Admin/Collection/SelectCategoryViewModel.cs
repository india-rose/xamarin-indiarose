using System.Collections.Generic;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class SelectCategoryViewModel : AbstractBrowserViewModel
	{
		private IndiagramContainer _currentIndiagram;

		[NavigationParameter]
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
                 {"CurrentIndiagram",new IndiagramContainer(indiagram)}
             });
		}
	}
}
