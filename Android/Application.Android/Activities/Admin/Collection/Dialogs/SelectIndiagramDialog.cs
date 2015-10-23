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

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[Activity(Label = "SelectIndiagramDialog")]
	public partial class SelectIndiagramDialog : AlertDialogFragmentBase
	{
		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Views_Admin_Collection_Dialogs_SelectIndiagramPage, container, false);
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Funq.Container.Locator.AdminCollectionDialogsSelectIndiagramDialog;
		}
	}
}