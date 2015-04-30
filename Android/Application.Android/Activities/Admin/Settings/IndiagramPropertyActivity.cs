#region Usings

using Android.App;
using Android.Content.PM;
using Android.OS;
using Storm.Mvvm;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class IndiagramPropertyActivity : ActivityBase
	{
		protected override void OnCreate(Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Admin_Settings_IndiagramPropertyPage);
            SetViewModel(Container.Locator.AdminSettingsIndiagramPropertyViewModel);
		}
	}
}