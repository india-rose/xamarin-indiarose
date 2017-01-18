using Android.App;
using Android.Content.PM;
using IndiaRose.Core.Admins.ViewModels;

namespace IndiaRose.Droid.Views.Settings
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape)]
	public class SettingsView : BaseActivity<SettingsViewModel>
	{
		public SettingsView() : base(Resource.Layout.SettingsView)
		{
		}
	}
}