#region Usings

using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class CategoryReadingViewModel : AbstractYesNoViewModel
	{
		#region Constructor

		public CategoryReadingViewModel(IContainer container) : base(container)
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
