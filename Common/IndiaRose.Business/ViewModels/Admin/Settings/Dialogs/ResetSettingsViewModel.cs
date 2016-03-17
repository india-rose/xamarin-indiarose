#region Usings



#endregion
namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    /// <summary>
    /// VueModèle du dialogue de réinitialisation des paramètres
    /// </summary>
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
