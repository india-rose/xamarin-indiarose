#region Usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		private IFontService _fontService;
		private IMessageDialogService _messageDialogService;

		protected IFontService FontService
		{
			get { return _fontService ?? (_fontService = Container.Resolve<IFontService>()); }
		}

		protected IMessageDialogService MessageDialogService
		{
			get { return _messageDialogService ?? (_messageDialogService = Container.Resolve<IMessageDialogService>()); }
		}

		#endregion

		#region Properties

		private string _fontName;
		private int _indiagramSize;
		private int _fontSize;
		private ColorContainer _reinforcerColor;

		public ColorContainer ReinforcerColor
		{
			get { return _reinforcerColor; }
			set { SetProperty(ref _reinforcerColor, value); }
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

		public IndiagramPropertyViewModel(IContainer container) : base(container)
		{
			ReinforcerColorCommand = new DelegateCommand(ReInforcerColorAction);

			ReinforcerColor = new ColorContainer
			{
				Color = SettingsService.ReinforcerColor
			};

			IndiagramSizes = new ObservableCollection<int>();
			//TODO : refactor to limit indiagram size with the height of the screen
			foreach (int size in new[] { 32, 48, 64, 80, 128, 160, 200, 256, 280, 300 })
			{
				IndiagramSizes.Add(size);
			}
			IndiagramSize = SettingsService.IndiagramDisplaySize;

			FontSizes = new ObservableCollection<int>();
			foreach (int size in new[] { 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 })
			{
				FontSizes.Add(size);
			}
			FontSize = SettingsService.FontSize;

			FontNames = new ObservableCollection<string>();
            IFontService fontService = Container.Resolve<IFontService>();
            foreach(string font in fontService.FontList.Keys)
            {
                FontNames.Add(font);
            }
            FontName = SettingsService.FontName;
		}

		private void ReInforcerColorAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, ReinforcerColor}
			});
		}

		protected override void SaveAction()
		{
			SettingsService.IndiagramDisplaySize = IndiagramSize;
			SettingsService.FontSize = FontSize;
			SettingsService.FontName = FontName;
			SettingsService.ReinforcerColor = ReinforcerColor.Color;

			base.SaveAction();
		}
    }
}