#region Usings 

using System;
using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    /// <summary>
    /// VueModèle de confirmation de suppresion d'une Category
    /// </summary>
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

		[NavigationParameter(Mode = NavigationParameterMode.Optional)]
		public Action DeleteCallback { get; set; }

		public DeleteCategoryWarningViewModel()
		{
			DeleteCommand = new DelegateCommand(DeleteAction);
		}

		protected void DeleteAction()
		{
            CloseDialogAction();
			if (Indiagram.HasChildren)
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETECONFIRMATION_CATEGORY, new Dictionary<string, object>
				{
					{"Indiagram", Indiagram},
					{"DeleteCallback", DeleteCallback}
				});
			}
			else
			{
				CollectionStorageService.Delete(Indiagram);
				if (DeleteCallback != null)
				{
					DeleteCallback();
				}
			}
		}
	}
}