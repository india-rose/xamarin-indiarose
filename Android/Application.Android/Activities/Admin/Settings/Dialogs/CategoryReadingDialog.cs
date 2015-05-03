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
	public partial class CategoryReadingDialog : AlertDialogFragmentBase
	{
		public CategoryReadingDialog()
		{
		    var translation = DependencyService.Container.Resolve<ILocalizationService>();

			Title = translation.GetString("CategoryReading_Title", "Text");
            Buttons.Add(DialogsButton.Positive, translation.GetString("Button_Ok", "Text"));
            Buttons.Add(DialogsButton.Negative, translation.GetString("Button_Cancel", "Text"));
		}

	    protected override ViewModelBase CreateViewModel()
	    {
			return Container.Locator.AdminSettingsDialogsCategoryReadingViewModel;
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
            return inflater.Inflate(Resource.Layout.Admin_Settings_Dialogs_CategoryReading, container, false);
		}
	}
}
