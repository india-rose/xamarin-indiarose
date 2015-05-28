﻿#region Usings 

using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
	public class ExploreCollectionIndiagramViewModel : AbstractCollectionViewModel
	{
		private Indiagram _indiagram;

		[NavigationParameter]
		public Indiagram Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

		public ICommand DeleteCommand { get; set; }
		public ICommand SeeIndiagramCommand { get; private set; }

		public ExploreCollectionIndiagramViewModel()
		{
			SeeIndiagramCommand = new DelegateCommand(WatchIndiagram);
			DeleteCommand = new DelegateCommand(DeleteAction);
		}

		private void DeleteAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING_INDIAGRAM, new Dictionary<string, object>
			{
				{"Indiagram", Indiagram}
			});
			CloseDialogAction();
		}

		private void WatchIndiagram()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCHINDIAGRAM, new Dictionary<string, object>
			{
				{"Indiagram", Indiagram}
			});
			CloseDialogAction();
		}
	}
}