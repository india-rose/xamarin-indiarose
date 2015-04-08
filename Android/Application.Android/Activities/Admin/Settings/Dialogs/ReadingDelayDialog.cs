#region Usings

using System;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "OkCommand", TargetPath = "PositiveButtonEvent")]
	[BindingElement(Path = "CancelCommand", TargetPath = "NegativeButtonEvent")]
	public partial class ReadingDelayDialog : AlertDialogFragmentBase
	{
		public ReadingDelayDialog()
		{
			Title = "Délai entre la lecture de deux mots";
			Buttons.Add(DialogsButton.Positive, "Ok");
			Buttons.Add(DialogsButton.Negative, "Cancel");
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminSettingsDialogsReadingDelayViewModel;
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			throw new NotImplementedException();
			return inflater.Inflate(Resource.Layout.Admin_Settings_Dialogs_ReadingDelay, container, false);
		}
	}
}