#region Usings

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
	public class IndiagramPropertyViewModel : AbstractBackViewModel
	{
		protected ISettingsService SettingsService;

		private int _currentIndiagramSize;

		public int CurrentIndiagramSize
		{
			get { return _currentIndiagramSize; }
			set { SetProperty(ref _currentIndiagramSize, value); }
		}

		private string _currentIndiagramFont;

		public string CurrentIndiagramFont
		{
			get { return _currentIndiagramFont; }
			set { SetProperty(ref _currentIndiagramFont, value); }
		}

		private int _currentIndiagramFontSize;

		public int CurrentIndiagramFontSize
		{
			get { return _currentIndiagramFontSize; }
			set { SetProperty(ref _currentIndiagramFontSize, value); }
		}

		public ICommand OkCommand { get; private set; }
		public ICommand ReinforcerColorCommand { get; private set; }
		public ObservableCollection<int> IndiagramsSize { get; private set; }
		public ObservableCollection<string> IndiagramsFont { get; private set; }
		public ObservableCollection<int> IndiagramsFontSize { get; private set; }
        public Dictionary<string, string> FontsList { get; private set; }

		public IndiagramPropertyViewModel(IContainer container)
			: base(container)
		{
			OkCommand = new DelegateCommand(OkAction);
			ReinforcerColorCommand = new DelegateCommand(ReInforcerColorAction);
			SettingsService = Container.Resolve<ISettingsService>();

			IndiagramsSize = new ObservableCollection<int>();
			foreach (int size in new int[] { 32, 48, 64, 80, 128, 160, 200, 256, 280, 300 })
			{
				IndiagramsSize.Add(size);
			}
			CurrentIndiagramSize = SettingsService.IndiagramDisplaySize;

			IndiagramsFontSize = new ObservableCollection<int>();
			foreach (int size in new int[] { 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 })
			{
				IndiagramsFontSize.Add(size);
			}
			CurrentIndiagramFontSize = SettingsService.FontSize;

			IndiagramsFont = new ObservableCollection<string>();
            IFontService fontService = Container.Resolve<IFontService>();
            FontsList = fontService.FontList;
            foreach(string font in FontsList.Keys)
            {
                IndiagramsFont.Add(font);
            }
            CurrentIndiagramFont = SettingsService.FontName;

		}

		private void ReInforcerColorAction()
		{
			IMessageDialogService messageDialogService = Container.Resolve<IMessageDialogService>();
			messageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>()
			{
				{ColorPickerViewModel.COLOR_KEY_PARAMETER, ColorPickerViewModel.REINFORCER_COLOR}
			});
		}

		private void OkAction()
		{
			SettingsService.Save();
			BackAction();
		}
    }
}