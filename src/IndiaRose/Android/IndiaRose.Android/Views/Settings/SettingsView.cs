using Android.App;
using IndiaRose.Core.Admins.ViewModels;

namespace IndiaRose.Droid.Views.Settings
{
	[Activity]
	public class SettingsView : BaseActivity<SettingsViewModel>
	{
		public SettingsView() : base(Resource.Layout.SettingsViewLayout)
		{
		}
	}
}