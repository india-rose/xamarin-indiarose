using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using IndiaRose.Business.ViewModels;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.User
{
	[Activity(Label = "India Rose User Dev", Icon = "@drawable/ir_logo", Theme = "@style/Theme.Sherlock.Light.NoActionBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape, LaunchMode = LaunchMode.SingleInstance, NoHistory = true)]
	public partial class UserSplashscreenActivity : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.UserSplashScreen);
			FindViewById<ImageView>(Resource.Id.SplashScreenLogo).SetImageResource(Resource.Drawable.logo);
			SetViewModel(new SplashScreenViewModel(SplashScreenViewModel.LaunchingType.User));
		}
	}
}