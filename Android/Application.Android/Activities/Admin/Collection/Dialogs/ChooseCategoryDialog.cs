using Android.App;
using Android.Content.PM;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class ChooseCategoryDialog : AlertDialogFragmentBase
	{
		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_ChooseCategoryPage, container, false);
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogsChooseCategoryViewModel;
		}
	}
}