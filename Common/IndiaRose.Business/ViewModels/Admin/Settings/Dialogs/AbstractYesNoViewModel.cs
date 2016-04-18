namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    /// <summary>
    /// VueModèle abstrait pour les boites des dialogues pour l'activation de certaines préférences
    /// </summary>
	public abstract class AbstractYesNoViewModel : AbstractSettingsViewModel
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
	}
}
