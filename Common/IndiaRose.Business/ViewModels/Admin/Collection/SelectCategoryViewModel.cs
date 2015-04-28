using System.Collections.Generic;
using IndiaRose.Data.Model;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class SelectCategoryViewModel : AbstractBrowserViewModel
	{
		private Indiagram _currentIndiagram;

		[NavigationParameter]
		public Indiagram CurrentIndiagram
		{
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SELECT, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram",indiagram}
             });
		}
	}
}
