#region Usings 

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

		public ICommand GoIntoCommand { get; set; }
		public ICommand SeeIndiagramCommand { get; private set; }

		public ExploreCollectionCategoryViewModel()
		{
			SeeIndiagramCommand = new DelegateCommand(WatchIndiagramAction);
			GoIntoCommand = new DelegateCommand(GoIntoAction);
		}

		private void GoIntoAction()
		{
			if (!Indiagram.HasChildren)
			{
				var trad = DependencyService.Container.Resolve<ILocalizationService>();
				var message = trad.GetString("Collection_CategoryEmpty", "Text");
				LazyResolver<IPopupService>.Service.DisplayPopup(message);
			}
			else
			{
				Messenger.Send(Messages.EXPLORE_COLLECTION_GOINTO_CATEGORY, Indiagram);
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