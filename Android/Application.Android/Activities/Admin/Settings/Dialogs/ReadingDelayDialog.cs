#region Usings

using System;
using System.Globalization;
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
	public partial class ReadingDelayDialog : AlertDialogFragmentBase
	{
		public ReadingDelayDialog()
		{
            var trad = DependencyService.Container.Resolve<ILocalizationService>();

            Title = trad.GetString("Dialogs_ReadingDelay", "Text");
            Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Ok", "Text"));
            Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminSettingsDialogsReadingDelayViewModel;
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
		    return inflater.Inflate(Resource.Layout.Admin_Settings_Dialogs_ReadingDelay, container, false);
		}
	}
}