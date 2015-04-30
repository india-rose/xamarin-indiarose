using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Storage;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class CollectionManagementViewModel : AbstractBrowserViewModel
	{
		public ICommand AddCommand { get; private set; }


		public CollectionManagementViewModel()
		{
			AddCommand = new DelegateCommand(AddAction);
		}

		private void AddAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD);
		}

		private void DisplayChildren(Category category)
		{
			DisplayedIndiagrams = category.Children;
		}
		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);
			if (indiagram.IsCategory)
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION, new Dictionary<string, object>()
			    {
				    {"Indiagram", new IndiagramContainer(indiagram)}
			    });
			else
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEINDIAGRAM, new Dictionary<string, object>()
			    {
				    {"Indiagram", new IndiagramContainer(indiagram)}
			    });
		}
	}
}
