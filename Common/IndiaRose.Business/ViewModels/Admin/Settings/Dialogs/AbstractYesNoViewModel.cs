using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class AbstractYesNoViewModel : AbstractSettingsViewModel
	{
		private bool _isEnabled = false;
        private bool _isDisabled = true;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (SetProperty(ref _isEnabled, value))
                {
                    IsDisabled = !value;
                }
            }
        }
		public bool IsDisabled
		{
            get { return _isDisabled; }
		    set
		    {
		        if (SetProperty(ref _isDisabled, value))
		        {
		            IsEnabled = !value;
		        }
		    }
		}

		public AbstractYesNoViewModel(IContainer container) : base(container)
		{
		}
	}
}
