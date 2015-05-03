#region Usings 

using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
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

		public DeleteCategoryConfirmationViewModel()
		{
			DeleteCommand = new DelegateCommand(DeleteAction);
		}

		protected void DeleteAction()
		{
			CollectionStorageService.Delete(Indiagram);
		}
	}
}