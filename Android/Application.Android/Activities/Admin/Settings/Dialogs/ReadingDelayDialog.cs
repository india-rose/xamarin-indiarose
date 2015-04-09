#region Usings

using System;
using System.Globalization;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "SaveCommand", TargetPath = "PositiveButtonEvent")]
	public partial class ReadingDelayDialog : AlertDialogFragmentBase
	{
		public ReadingDelayDialog()
		{
            //a changer maniere trad
            var ci = CultureInfo.InstalledUICulture;
            if (ci.TwoLetterISOLanguageName == "fr")
            {
                Buttons.Add(DialogsButton.Positive, "Ok");
                Buttons.Add(DialogsButton.Negative, "Annuler");
            }
            else
            {
                Title = "Delay between two words"; ;
                Buttons.Add(DialogsButton.Positive, "Ok");
                Buttons.Add(DialogsButton.Negative, "Back");
            }
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminSettingsDialogsReadingDelayViewModel;
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			//throw new NotImplementedException();
		    return inflater.Inflate(Resource.Layout.Admin_Settings_Dialogs_ReadingDelay, container, false);
		}
	}
}