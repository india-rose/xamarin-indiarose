using System.Collections.Generic;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class ManagementViewModel : AbstractViewModel
    {

        public ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
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
            TextColor = new ColorContainer
            {
                Color = SettingsService.TextColor
            };

			Displayed = new List<Indiagram>()
			{
				new Indiagram("helloa", ""),
				new Indiagram("helloz", ""),
				new Indiagram("helloe", ""),
				new Indiagram("hellor", ""),
				new Indiagram("hellot", ""),
				new Indiagram("helloy", ""),
				new Indiagram("hellou", ""),
				new Indiagram("helloi", ""),
				new Indiagram("helloo", ""),
				new Indiagram("hellop", ""),
				new Indiagram("helloq", ""),
				new Indiagram("hellos", ""),
				new Indiagram("hellod", ""),
				new Indiagram("hellof", ""),
				new Indiagram("hellog", ""),
				new Indiagram("helloh", ""),
				new Indiagram("helloj", ""),
				new Indiagram("hellok", ""),
				new Indiagram("hellol", ""),
				new Indiagram("hellom", ""),
				new Indiagram("hellow", ""),
			};
        }
    }
}
