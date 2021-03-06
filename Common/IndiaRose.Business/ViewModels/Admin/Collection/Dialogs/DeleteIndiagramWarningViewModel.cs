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
    /// VueModèle du dialogue de confirmation d'un Indiagram
    /// </summary>
	public class DeleteIndiagramWarningViewModel : AbstractCollectionViewModel
	{
		private Indiagram _indiagram;
		public ICommand DeleteCommand { get; private set; }

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

		[NavigationParameter(Mode = NavigationParameterMode.Optional)]
		public Action DeleteCallback { get; set; }

		public DeleteIndiagramWarningViewModel()
		{
			DeleteCommand = new DelegateCommand(DeleteAction);
		}

        /// <summary>
        /// Supprime l'Indiagram de la Collection
        /// </summary>
		protected void DeleteAction()
		{
            CloseDialogAction();
			CollectionStorageService.Delete(Indiagram);

			if (DeleteCallback != null)
			{
				DeleteCallback();
			}
		}
	}
}