#region Usings 

using System;
using System.Collections.Generic;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class WatchIndiagramViewModel : AbstractCollectionViewModel
	{
        private IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }
        private ITextToSpeechService TtsService
        {
            get { return LazyResolver<ITextToSpeechService>.Service; }
        }
		private IndiagramContainer _indiagramContainer;
		public ICommand EditCommand { get; private set; }
		public ICommand DeleteCommand { get; private set; }
        public ICommand ListenCommand { get; private set; }

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
            ListenCommand = new DelegateCommand(ListenAction);
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
			if (IndiagramContainer.Indiagram.IsCategory)
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING_CATEGORY, new Dictionary<string, object>
				{
					{"Indiagram", IndiagramContainer.Indiagram},
					{"DeleteCallback", (Action)OnDeleted}
				});
			}
			else
			{
				MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_DELETEWARNING_INDIAGRAM, new Dictionary<string, object>
				{
					{"Indiagram", IndiagramContainer.Indiagram},
					{"DeleteCallback", (Action)OnDeleted}
				});
			}
		}

		private void OnDeleted()
		{
			NavigationService.GoBack();
		}

	    private void ListenAction()
        {
            if (IndiagramContainer==null)return;
			TtsService.PlayIndiagram(IndiagramContainer.Indiagram);
	    }
	}
}