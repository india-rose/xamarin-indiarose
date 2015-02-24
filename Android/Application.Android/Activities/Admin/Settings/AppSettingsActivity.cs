#region Usings

using Android.App;
using Android.Content.PM;
using Android.OS;
using IndiaRose.Business;
using Storm.Mvvm;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class AppSettingsActivity : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Admin_Settings_AppSettingsPage);
			SetViewModel(Container.Locator.AdminSettingsAppSettingsViewModel);
		}
	}
}