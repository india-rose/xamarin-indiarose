using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using IndiaRose.Business.ViewModels;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.Admin
{
	[Activity(Label = "India Rose Dev", MainLauncher = false, Icon = "@drawable/ir_logo_params", ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar", LaunchMode = LaunchMode.SingleInstance, NoHistory = true)]
	public partial class AdminSplashscreenActivity : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.AdminSplashScreen);
			FindViewById<ImageView>(Resource.Id.SplashScreenLogo).SetImageResource(Resource.Drawable.logo_admin);
			SetViewModel(new SplashScreenViewModel(SplashScreenViewModel.LaunchingType.Admin));
		}
	}
}