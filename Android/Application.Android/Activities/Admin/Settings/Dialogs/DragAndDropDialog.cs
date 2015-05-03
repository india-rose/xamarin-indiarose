#region Usings

using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "SaveCommand", TargetPath = "PositiveButtonEvent")]
	public partial class DragAndDropDialog : AlertDialogFragmentBase
	{
		public DragAndDropDialog()
		{
		    var trad = DependencyService.Container.Resolve<ILocalizationService>();

			Title = trad.GetString("DragAndDrop_Title", "Text");
            Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Ok", "Text"));
            Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Cancel", "Text"));
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminSettingsDialogsDragAndDropViewModel;
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
            return inflater.Inflate(Resource.Layout.Admin_Settings_Dialogs_DragAndDrop, container, false);
		}
	}
}