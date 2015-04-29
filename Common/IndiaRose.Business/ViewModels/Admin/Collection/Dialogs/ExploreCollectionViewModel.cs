﻿using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ExploreCollectionViewModel : AbstractSettingsViewModel
    {
		private IndiagramContainer _indiagram;

		[NavigationParameter]
		public IndiagramContainer Indiagram
		{
			get { return _indiagram; }
			set { SetProperty(ref _indiagram, value); }
		}

        public ICommand WatchIndiagramCommand{ get; private set; }
		public ICommand DeleteCommand { get; set; }
        public ExploreCollectionViewModel()
        {
            WatchIndiagramCommand = new DelegateCommand(WatchIndiagram);
	        DeleteCommand = new DelegateCommand(DeleteAction);
        }

	    private void DeleteAction()
	    {
		    //Delete();
	    }
        private void WatchIndiagram()
        {
			NavigationService.Navigate(Views.ADMIN_COLLECTION_WATCH, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram", Indiagram}
             });
        }
    }
}
