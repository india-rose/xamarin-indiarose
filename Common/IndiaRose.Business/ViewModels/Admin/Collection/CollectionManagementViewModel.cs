#region Usings 

using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Messaging;

#endregion

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
			// parameter can be removed when the framework has been updated
			NavigationService.Navigate(Views.ADMIN_COLLECTION_ADDINDIAGRAM, new Dictionary<string, object>
			{
				{"Indiagram", null}
			});
		}

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);
			if (indiagram.IsCategory)
			{
				// register to message go into category from the explore collection dialog
				// and unregister when the dialog is dismissed
				Messenger.Register<Category>(Messages.EXPLORE_COLLECTION_GOINTO_CATEGORY, this, PushCategory);
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_CATEGORY, new Dictionary<string, object>
				{
					{"Indiagram", indiagram}
				}, () => Messenger.Unregister(this, Messages.EXPLORE_COLLECTION_GOINTO_CATEGORY));
			}
			else
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_INDIAGRAM, new Dictionary<string, object>
				{
					{"Indiagram", indiagram}
				});
			}
		}
	}
}