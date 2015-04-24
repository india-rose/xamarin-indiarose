using System.Collections.Generic;
using System.Windows.Input;
using Storm.Mvvm.Commands;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using IndiaRose.Storage;
using IndiaRose.Storage.Sqlite;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class ManagementViewModel : AbstractViewModel
    {
		public IMessageDialogService MessageDialogService
		{
			get { return LazyResolver<IMessageDialogService>.Service; }
		}

        public ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }

        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }

	    private int _collectionOffset;
	    private int _displayCount;
	    private List<Indiagram> _displayed;
        private ColorContainer _textColor;
        public ColorContainer TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

	    public int CollectionOffset
	    {
			get { return _collectionOffset;}
			set { SetProperty(ref _collectionOffset, value); }
	    }

	    public int DisplayCount
	    {
			get { return _displayCount; }
			set { SetProperty(ref _displayCount, value); }
	    }

	    public List<Indiagram> Displayed
	    {
			get { return _displayed; }
			set { SetProperty(ref _displayed, value); }
	    }

        public List<Indiagram> CurrentIndiagram { get; private set; }

        public ManagementViewModel()
        {
	        CollectionManagementViewModel.SubViewModel = this;

            TextColor = new ColorContainer
            {
                Color = SettingsService.TextColor
            };

	        if (LazyResolver<ICollectionStorageService>.Service.GetTopLevel().Count == 0)
	        {
		        LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("azerty", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloa", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloz", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloe", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellor", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellot", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloy", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellou", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloi", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloo", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellop", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloq", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellos", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellod", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellof", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellog", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloh", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("helloj", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellok", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellol", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellom", ""));
				LazyResolver<ICollectionStorageService>.Service.Create(new Indiagram("hellow", ""));
	        }
			Displayed = LazyResolver<ICollectionStorageService>.Service.GetTopLevel();
        }

	    public void NotifyNextAction()
	    {
		    int offset = CollectionOffset;
		    offset += DisplayCount;

		    if (offset >= Displayed.Count)
		    {
			    offset = 0;
		    }
		    CollectionOffset = offset;
	    }

		public void AddCollectionAction(Indiagram Indiagram)
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_ADDCOLLECTION, new Dictionary<string, object>()
             {
                 {"indiagram",Indiagram}
             });
		}

    }
}
