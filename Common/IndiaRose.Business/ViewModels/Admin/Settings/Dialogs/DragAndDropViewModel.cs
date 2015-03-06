#region Usings

using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class DragAndDropViewModel : AbstractYesNoViewModel
	{
		#region Constructor

		public DragAndDropViewModel(IContainer container) : base(container)
		{
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