#region Usings

using Storm.Mvvm.Inject;

#endregion
namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    public class ResetSettingsViewModel : AbstractSettingsViewModel
    {
        #region Constructor
        public ResetSettingsViewModel(IContainer container) : base(container)
        {

        }

        #endregion

        #region Commands implementation

	    protected override void SaveAction()
	    {
			SettingsService.Reset();
		    base.SaveAction();
	    }

        #endregion
    }
}
