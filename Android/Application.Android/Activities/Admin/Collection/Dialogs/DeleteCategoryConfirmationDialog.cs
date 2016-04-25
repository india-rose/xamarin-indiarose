using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[BindingElement(Path = "DeleteCommand", TargetPath = "PositiveButtonEvent")]
	public partial class DeleteCategoryConfirmationDialog : AlertDialogFragmentBase
	{
		public DeleteCategoryConfirmationDialog()
		{
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
            Title = trad.GetString("DeleteCategoryConfirmation_Title", "Text");
		    Message = trad.GetString("DeleteCategoryConfirmation_Explanation","Text");
            
            Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Delete", "Text"));
			Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Cancel", "Text"));
		}
		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return null;
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogsDeleteCategoryConfirmationViewModel;
		}
	}
}