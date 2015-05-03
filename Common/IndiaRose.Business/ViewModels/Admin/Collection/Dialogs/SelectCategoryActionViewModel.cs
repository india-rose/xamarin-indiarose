#region Usings 

using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Messaging;
using Storm.Mvvm.Navigation;

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
			Messenger.Send<Indiagram>(Messages.SELECT_CATEGORY_GOINTO_CATEGORY, Indiagram);
		}

		private void SendIndiagramAction()
		{
			Messenger.Send<Indiagram>(Messages.SELECT_CATEGORY_SELECTED_CATEGORY, Indiagram);
		}
	}
}