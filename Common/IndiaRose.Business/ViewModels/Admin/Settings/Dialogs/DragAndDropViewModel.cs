#region Usings



#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    /// <summary>
    /// VueModèle pour le dialogue du Drag and Drop
    /// </summary>
	public class DragAndDropViewModel : AbstractYesNoViewModel
	{
		#region Constructor

		public DragAndDropViewModel()
        {
            //Initilisation de la propriété déjà connu
			IsEnabled = SettingsService.IsDragAndDropEnabled;
		}

		#endregion

		#region Commands implementation

		protected override void SaveAction()
		{
			SettingsService.IsDragAndDropEnabled = IsEnabled;
			base.SaveAction();
		}

		#endregion
	}
}