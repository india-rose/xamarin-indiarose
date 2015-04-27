using Android.App;
using Android.OS;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[Activity(Label = "SelectManagementDialog")]
	public partial class SelectManagementDialog : AlertDialogFragmentBase
	{
		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_SelectManagementDialog, container, false);
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogSelectManagementViewModel;
		}
	}
}