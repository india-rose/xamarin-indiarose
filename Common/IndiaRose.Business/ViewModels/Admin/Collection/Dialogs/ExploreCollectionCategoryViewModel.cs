#region Usings 

using System;
using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Messaging;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
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
			}
		}

		private void WatchIndiagramAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCHINDIAGRAM, new Dictionary<string, object>
			{
				{"Indiagram", Indiagram}
			});
		}
	}
}