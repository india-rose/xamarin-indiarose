#region Usings



#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    /// <summary>
    /// VueModèle pour le dialogue pour la lecture des catégorie
    /// </summary>
	public class CategoryReadingViewModel : AbstractYesNoViewModel
	{
		#region Constructor

		public CategoryReadingViewModel()
		{
            //Initilisation de la propriété déjà connu
			IsEnabled = SettingsService.IsCategoryNameReadingEnabled;
		}

		#endregion

		#region Commands implementation

		protected override void SaveAction()
		{
			SettingsService.IsCategoryNameReadingEnabled = IsEnabled;
			base.SaveAction();
		}

		#endregion
	}
}
