using Android.App;
using Android.Widget;
using IndiaRose.Core.Admins.ViewModels;
using ReactiveUI;

namespace IndiaRose.Droid.Views
{
	[Activity(MainLauncher = true, Label = "India Rose", Icon = "@drawable/Icon")]
	public class HomeActivity : BaseActivity<HomeViewModel>
	{
		public HomeActivity() : base(Resource.Layout.HomeViewLayout)
		{
		}

		protected override void BindControls()
		{
			base.BindControls();

			Button navigationButton = FindViewById<Button>(Resource.Id.Button);

			ReactiveCommand command = null;
			
			navigationButton.Events().Click.Subscribe(_ => ViewModel.OpenAppSettingsCommand)
		}
	}
}