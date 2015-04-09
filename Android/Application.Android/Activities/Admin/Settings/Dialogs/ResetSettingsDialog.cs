#region Usings

using System.Globalization;
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
            var ci = CultureInfo.InstalledUICulture;
            if (ci.TwoLetterISOLanguageName == "fr")
            {
                Title = "Êtes-vous sûr de vouloir réinitialiser les paramètres ?";
                Buttons.Add(DialogsButton.Positive, "Ok");
                Buttons.Add(DialogsButton.Negative, "Annuler");
            }
            else
            {
                Title = "Do you want to reset parameters ?";
                Buttons.Add(DialogsButton.Positive, "Ok");
                Buttons.Add(DialogsButton.Negative, "Back");
            }
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