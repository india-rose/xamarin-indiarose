#region Usings

using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
    [BindingElement(Path = "SaveCommand", TargetPath = "PositiveButtonEvent")]
    public partial class ResetSettingsDialog : AlertDialogFragmentBase
    {
        public ResetSettingsDialog()
        {
            Title ="Êtes-vous sûr de vouloir réinitialiser les paramètres ?";
            Buttons.Add(DialogsButton.Positive, "Ok");
            Buttons.Add(DialogsButton.Negative, "Annuler");
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminSettingsDialogsResetSettingsViewModel;
        }

        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
			// No view necessary for this one
	        return null;
        }
    }
}