using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Messaging;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class CollectionManagementViewModel : AbstractBrowserViewModel
	{
		public CollectionManagementViewModel()
		{
			AddCommand = new DelegateCommand(AddAction);
		}

		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			base.OnNavigatedTo(e, parametersKey);

			Messenger.Register<Category>(Messages.ADMIN_COLLECTION_GOINTO, this, DisplayChildren);
		}

		public override void OnNavigatedFrom(NavigationArgs e)
		{
			base.OnNavigatedFrom(e);

			Messenger.Unregister(this, Messages.ADMIN_COLLECTION_GOINTO);
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