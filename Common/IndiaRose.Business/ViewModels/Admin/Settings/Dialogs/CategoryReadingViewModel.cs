#region Usings



#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class CategoryReadingViewModel : AbstractYesNoViewModel
	{
		#region Constructor

		public CategoryReadingViewModel()
		{
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
