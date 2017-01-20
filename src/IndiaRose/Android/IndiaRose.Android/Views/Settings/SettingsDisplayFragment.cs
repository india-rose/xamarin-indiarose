using IndiaRose.Core.Admins.ViewModels;

namespace IndiaRose.Droid.Views.Settings
{
	public class SettingsDisplayFragment : BaseFragment<SettingsViewModel>
	{
		public SettingsDisplayFragment() : base(Resource.Layout.SettingsDisplayFragment)
		{
			
		}

		protected override SettingsViewModel CreateViewModel()
		{
			return null; //on purpose, will be affected from parent
		}
	}
}