#region Usings

using Android.App;
using Android.Content.PM;
using Android.OS;
using IndiaRose.Business;
using Storm.Mvvm;

#endregion

namespace IndiaRose.Application.Activities.Admin
{
	[Activity(Label = "Application.Android", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class HomeActivity : ActivityBase
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Admin_HomePage);
			SetViewModel(Container.Locator.AdminHomeViewModel);
		}
	}
}