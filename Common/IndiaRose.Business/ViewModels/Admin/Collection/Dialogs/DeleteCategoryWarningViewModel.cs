#region Usings 

using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
	public class DeleteCategoryWarningViewModel : AbstractCollectionViewModel
	{
		private Indiagram _indiagram;
		public ICommand DeleteCommand { get; set; }

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

		public DeleteCategoryWarningViewModel()
		{
			DeleteCommand = new DelegateCommand(DeleteAction);
		}

		protected void DeleteAction()
		{
			if (Indiagram.HasChildren)
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETECONFIRMATION_CATEGORY, new Dictionary<string, object>
				{
					{"Indiagram", Indiagram}
				});
			}
			else
			{
				CollectionStorageService.Delete(Indiagram);
			}
		}
	}
}