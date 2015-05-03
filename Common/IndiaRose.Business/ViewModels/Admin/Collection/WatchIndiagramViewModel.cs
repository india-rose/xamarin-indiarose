#region Usings 

using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class WatchIndiagramViewModel : AbstractCollectionViewModel
	{
		private IndiagramContainer _indiagramContainer;
		public ICommand EditCommand { get; private set; }
		public ICommand DeleteCommand { get; private set; }

		[NavigationParameter]
		public Indiagram Indiagram
		{
			set
			{
				if (value != null)
				{
					IndiagramContainer = new IndiagramContainer(value);
				}
			}
		}

		public IndiagramContainer IndiagramContainer
		{
			get { return _indiagramContainer; }
			set { SetProperty(ref _indiagramContainer, value); }
		}

		public WatchIndiagramViewModel()
		{
			EditCommand = new DelegateCommand(EditAction);
			DeleteCommand = new DelegateCommand(DeleteAction);
		}

		private void EditAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_ADDINDIAGRAM, new Dictionary<string, object>
			{
				{"Indiagram", IndiagramContainer}
			});
		}

		protected void DeleteAction()
		{
			//TODO: handle back when indiagram are deleted!
			if (IndiagramContainer.Indiagram.IsCategory)
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING_CATEGORY, new Dictionary<string, object>
				{
					{"Indiagram", IndiagramContainer.Indiagram}
				});
			}
			else
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING_INDIAGRAM, new Dictionary<string, object>
				{
					{"Indiagram", IndiagramContainer.Indiagram}
				});
			}
		}
	}
}