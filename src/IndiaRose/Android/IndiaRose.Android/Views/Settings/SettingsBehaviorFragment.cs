using IndiaRose.Core.Admins.ViewModels;

namespace IndiaRose.Droid.Views.Settings
{
	public class SettingsBehaviorFragment : BaseFragment<SettingsViewModel>
	{
		public SettingsBehaviorFragment() : base(Resource.Layout.SettingsBehaviorFragment)
		{

		}

		protected override SettingsViewModel CreateViewModel()
		{
			return null; //on purpose, will be affected from parent
		}
	}
}