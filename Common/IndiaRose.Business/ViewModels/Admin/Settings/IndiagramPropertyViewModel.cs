#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
	public class IndiagramPropertyViewModel : AbstractSettingsViewModel
	{
		#region Services

		protected IFontService FontService
		{
			get { return LazyResolver<IFontService>.Service; }
		}

		protected IMessageDialogService MessageDialogService
		{
			get { return LazyResolver<IMessageDialogService>.Service; }
		}

		#endregion

		#region Properties

		private string _fontName;
		private int _indiagramSize;
		private int _fontSize;
		private ColorContainer _reinforcerColor;
	    private ColorContainer _textColor;
		private bool _backAfterSelection;
		private bool _reinforcerEnabled;

	    public bool BackAfterSelection
	    {
            get { return _backAfterSelection; }
	        set { SetProperty(ref _backAfterSelection, value); }
	    }
        public bool ReinforcerEnabled
        {
            get { return _reinforcerEnabled; }
			set { SetProperty(ref _reinforcerEnabled, value); }
        }

		public ColorContainer ReinforcerColor
		{
			get { return _reinforcerColor; }
			set { SetProperty(ref _reinforcerColor, value); }
		}

	    public ColorContainer TextColor
	    {
			get { return _textColor; }
			set { SetProperty(ref _textColor, value); }
	    }

		public int IndiagramSize
		{
			get { return _indiagramSize; }
			set { SetProperty(ref _indiagramSize, value); }
		}

		public string FontName
		{
			get { return _fontName; }
			set { SetProperty(ref _fontName, value); }
		}

		public int FontSize
		{
			get { return _fontSize; }
			set { SetProperty(ref _fontSize, value); }
		}

		public ObservableCollection<int> IndiagramSizes { get; private set; }
		public ObservableCollection<string> FontNames { get; private set; }
		public ObservableCollection<int> FontSizes { get; private set; }

		#endregion

		public ICommand ReinforcerColorCommand { get; private set; }
        public ICommand TextColorCommand { get; private set; }

	    public IndiagramPropertyViewModel()
	    {
	        ReinforcerColorCommand = new DelegateCommand(ReInforcerColorAction);
            TextColorCommand = new DelegateCommand(TextColorAction);

	        ReinforcerColor = new ColorContainer
	        {
	            Color = SettingsService.ReinforcerColor
            }; 
            TextColor = new ColorContainer
            {
                Color = SettingsService.TextColor
            };
		    ReinforcerEnabled = SettingsService.IsReinforcerEnabled;
		    BackAfterSelection = SettingsService.IsBackHomeAfterSelectionEnabled;

	        IndiagramSizes = new ObservableCollection<int>();
	        //TODO : refactor to limit indiagram size with the height of the screen
	        foreach (int size in new[] {32, 48, 64, 80, 128, 160, 200, 256, 280, 300})
	        {
	            IndiagramSizes.Add(size);
	        }
	        IndiagramSize = SettingsService.IndiagramDisplaySize;

	        FontSizes = new ObservableCollection<int>();
	        foreach (int size in new[] {8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32})
	        {
	            FontSizes.Add(size);
	        }
	        FontSize = SettingsService.FontSize;

	        FontNames = new ObservableCollection<string>();
	        foreach (string font in FontService.FontList.Keys)
	        {
	            FontNames.Add(font);
	        }
		    if (FontService.FontList.ContainsValue(SettingsService.FontName))
		    {
			    FontName = FontService.FontList.First(
				    x => string.Equals(x.Value, SettingsService.FontName, StringComparison.OrdinalIgnoreCase)).Key;
		    }
		    else
		    {
			    FontName = FontService.FontList.First().Key;
		    }
	    }

	    private void ReInforcerColorAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, ReinforcerColor}
			});
		}

	    private void TextColorAction()
	    {
            MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, TextColor}
			});
	    }

		protected override void SaveAction()
		{
			SettingsService.IndiagramDisplaySize = IndiagramSize;
			SettingsService.FontSize = FontSize;
			SettingsService.FontName = FontService.FontList[FontName];
			SettingsService.ReinforcerColor = ReinforcerColor.Color;
		    SettingsService.TextColor = TextColor.Color;

			base.SaveAction();
            BackAction();
		}
    }
}