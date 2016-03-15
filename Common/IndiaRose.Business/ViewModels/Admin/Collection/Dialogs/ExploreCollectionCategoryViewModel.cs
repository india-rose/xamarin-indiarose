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
    /// VueModèle du dialogue suivant un touch sur une Catégorie (sur la page de navigation dans la collection dans la partie Administrateur)
    /// </summary>
	public class ExploreCollectionCategoryViewModel : AbstractCollectionViewModel
	{
		private Indiagram _indiagram;

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

		[NavigationParameter(Mode = NavigationParameterMode.Optional)]
		public Action<Category> GoIntoCallback { get; set; } 

		public ICommand GoIntoCommand { get; set; }
		public ICommand SeeIndiagramCommand { get; private set; }

		public ExploreCollectionCategoryViewModel()
		{
			SeeIndiagramCommand = new DelegateCommand(WatchIndiagramAction);
			GoIntoCommand = new DelegateCommand(GoIntoAction);
		}

		private void GoIntoAction()
		{
			if (GoIntoCallback != null)
			{
				GoIntoCallback(Indiagram as Category);
				CloseDialogAction();
			}
		}

		private void WatchIndiagramAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCHINDIAGRAM, new Dictionary<string, object>
			{
				{"Indiagram", Indiagram}
			});
			CloseDialogAction();
		}
	}
}