#region Usings



#endregion
namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    public class ResetSettingsViewModel : AbstractSettingsViewModel
    {
        #region Commands implementation

	    protected override void SaveAction()
	    {
			SettingsService.Reset();
		    base.SaveAction();
	    }

        #endregion
    }
}
