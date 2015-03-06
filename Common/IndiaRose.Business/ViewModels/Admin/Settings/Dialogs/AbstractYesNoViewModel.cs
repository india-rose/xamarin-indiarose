using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class AbstractYesNoViewModel : AbstractSettingsViewModel
	{
		private bool _isEnabled;

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { SetProperty(ref _isEnabled, value); }
		}

		public AbstractYesNoViewModel(IContainer container) : base(container)
		{
		}
	}
}
