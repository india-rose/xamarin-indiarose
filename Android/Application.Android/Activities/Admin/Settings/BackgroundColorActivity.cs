#region Usings

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class BackgroundColorActivity : ActivityBase
	{
	    protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Admin_Settings_BackgroundColorPage);
	        SetViewModel(Container.Locator.AdminSettingsBackgroundColorViewModel);
		}
	}
}