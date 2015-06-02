#region Usings 

using System;
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
	public class SelectCategoryActionViewModel : AbstractCollectionViewModel
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

		[NavigationParameter(Mode = NavigationParameterMode.Optional)]
		public Action<Category> SelectedCallback { get; set; } 

		public ICommand SelectCommand { get; private set; }
		public ICommand GoIntoCommand { get; private set; }

		public SelectCategoryActionViewModel()
		{
			SelectCommand = new DelegateCommand(SendIndiagramAction);
			GoIntoCommand = new DelegateCommand(GoIntoAction);
		}

		private void GoIntoAction()
		{
            CloseDialogAction();

			if (GoIntoCallback != null)
			{
				GoIntoCallback(Indiagram as Category);
			}
		}

		private void SendIndiagramAction()
		{
            CloseDialogAction();

			if (SelectedCallback != null)
			{
				SelectedCallback(Indiagram as Category);
			}
		}
	}
}