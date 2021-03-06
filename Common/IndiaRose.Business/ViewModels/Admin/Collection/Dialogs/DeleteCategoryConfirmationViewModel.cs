﻿#region Usings 

using System;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    /// <summary>
    /// VueModèle de la deuxième confirmation lors de la suppresion d'une Catégorie.
    /// Arrive lorsque la catégorie a des enfants
    /// </summary>
	public class DeleteCategoryConfirmationViewModel : AbstractCollectionViewModel
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

		public DeleteCategoryConfirmationViewModel()
		{
			DeleteCommand = new DelegateCommand(DeleteAction);
		}

        /// <summary>
        /// Supprime la Categorie de la collection et lance le callback de suppression
        /// </summary>
		protected void DeleteAction()
		{
			CollectionStorageService.Delete(Indiagram);
			if (DeleteCallback != null)
			{
				DeleteCallback();
			}
            CloseDialogAction();
		}
	}
}