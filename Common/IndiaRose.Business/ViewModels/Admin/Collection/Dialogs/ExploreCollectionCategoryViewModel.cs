#region Usings 

using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Messaging;
using Storm.Mvvm.Navigation;

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

		public ICommand GoIntoCommand { get; set; }
		public ICommand SeeIndiagramCommand { get; private set; }

		public ExploreCollectionCategoryViewModel()
		{
			SeeIndiagramCommand = new DelegateCommand(WatchIndiagramAction);
			GoIntoCommand = new DelegateCommand(GoIntoAction);
		}

		private void GoIntoAction()
		{
			Messenger.Send(Messages.EXPLORE_COLLECTION_GOINTO_CATEGORY, Indiagram);
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