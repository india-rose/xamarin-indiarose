using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[Activity(Label = "ConfirmationDelete")]
	public partial class ConfirmationDeleteDialog : AlertDialogFragmentBase
	{
		public ConfirmationDeleteDialog()
		{
			var trad = DependencyService.Container.Resolve<ILocalizationService>();
			Title = trad.GetString("ConfirmeDeletion", "Text");

			Buttons.Add(DialogsButton.Neutral, trad.GetString("Indiagram_Property_cancel", "Text"));
			Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Ok", "Text"));
		}
		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_ConfirmationDeleteDialog, container, false);
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogsExploreCollectionDialog;
		}
	}
}