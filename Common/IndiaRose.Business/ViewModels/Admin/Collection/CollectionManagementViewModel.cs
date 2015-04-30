using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Messaging;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class CollectionManagementViewModel : AbstractBrowserViewModel
	{
		public CollectionManagementViewModel()
		{
			AddCommand = new DelegateCommand(AddAction);
		}

		public ICommand AddCommand { get; private set; }

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
			{
				Messenger.Register<Category>(Messages.ADMIN_COLLECTION_GOINTO, this, DisplayChildren);
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION, new Dictionary<string, object>
				{
					{"Indiagram", new IndiagramContainer(indiagram)}
				});
			}
			else
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEINDIAGRAM, new Dictionary<string, object>
				{
					{"Indiagram", new IndiagramContainer(indiagram)}
				});
			}
		}
	}
}