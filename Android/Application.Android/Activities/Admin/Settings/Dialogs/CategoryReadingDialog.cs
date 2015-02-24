#region Usings

using Android.Views;
using IndiaRose.Business;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "OkCommand", TargetPath = "PositiveButtonEvent")]
	[BindingElement(Path = "CancelCommand", TargetPath = "NegativeButtonEvent")]
	public partial class CategoryReadingDialog : AlertDialogFragmentBase
	{
		public CategoryReadingDialog()
		{
			Title = "Lire le son associé à la catégorie lors de la sélection ?";
			Buttons.Add(DialogsButton.Positive, "Ok");
			Buttons.Add(DialogsButton.Negative, "Annuler");
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
