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
		    var trad = DependencyService.Container.Resolve<ILocalizationService>();

            Title = trad.GetString("Dialogs_ReadingDialog", "Text");
            Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Ok", "Text"));
            Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
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
