#region Usings 

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

		public ICommand SelectCommand { get; private set; }
		public ICommand GoIntoCommand { get; private set; }

		public SelectCategoryActionViewModel()
		{
			SelectCommand = new DelegateCommand(SendIndiagramAction);
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
				Messenger.Send<Indiagram>(Messages.SELECT_CATEGORY_GOINTO_CATEGORY, Indiagram);
			}
		}

		private void SendIndiagramAction()
		{
			Messenger.Send<Indiagram>(Messages.SELECT_CATEGORY_SELECTED_CATEGORY, Indiagram);
		}
	}
}