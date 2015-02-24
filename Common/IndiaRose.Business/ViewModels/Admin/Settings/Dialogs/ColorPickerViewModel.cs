#region Usings

using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class ColorPickerViewModel : ViewModelBase
	{
		public const string COLOR_KEY_PARAMETER = "ColorKey";
		public const string TOP_AREA_BACKGROUND_COLOR = "TOP";
		public const string BOTTOM_AREA_BACKGROUND_COLOR = "BOTTOM";
		public const string REINFORCER_COLOR = "REINFORCER";

	    private uint _currentColor;
	    private uint _oldColor;

		private readonly ISettingsService _settingsService;

		[NavigationParameter]
		public string ColorKey { get; set; }

		public ICommand PositiveCommand { get; set; }

	    public uint CurrentColor
	    {
            get { return _currentColor; }
            set { SetProperty(ref _currentColor, value); }
	    }

	    public uint OldColor
	    {
            get { return _oldColor; }
            set { SetProperty(ref _oldColor, value); }
	    }

	    public ColorPickerViewModel(IContainer container) : base(container)
		{
			PositiveCommand = new DelegateCommand(PositiveAction);

		    _settingsService = container.Resolve<ISettingsService>();
		}

	    public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
	    {
	        base.OnNavigatedTo(e, parametersKey);

		    if (ColorKey == TOP_AREA_BACKGROUND_COLOR)
		    {
			    OldColor = _settingsService.TopBackgroundColor;
		    }
			else if (ColorKey == BOTTOM_AREA_BACKGROUND_COLOR)
			{
				OldColor = _settingsService.BottomBackgroundColor;
			}
			else if (ColorKey == REINFORCER_COLOR)
			{
				OldColor = _settingsService.ReinforcerColor;
			}
	    }

		private void PositiveAction()
		{
			if (ColorKey == TOP_AREA_BACKGROUND_COLOR)
			{
				_settingsService.TopBackgroundColor = CurrentColor;
			}
			else if (ColorKey == BOTTOM_AREA_BACKGROUND_COLOR)
			{
				_settingsService.BottomBackgroundColor = CurrentColor;
			}
			else if (ColorKey == REINFORCER_COLOR)
			{
				_settingsService.ReinforcerColor = CurrentColor;
			}
		}
	}
}