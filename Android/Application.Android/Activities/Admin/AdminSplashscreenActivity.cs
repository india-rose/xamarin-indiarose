using Android.App;
using Android.Content.PM;
using Android.OS;
using IndiaRose.Services.Android.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Inject;

namespace IndiaRose.Application.Activities.Admin
{
	[Activity(Label = "India Rose Dev", MainLauncher = true, Icon = "@drawable/ir_logo_params", ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar", LaunchMode = LaunchMode.SingleInstance, NoHistory = true)]
	public class AdminSplashscreenActivity : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.AdminSplashScreen);

			LazyResolver<IInitializationStateService>.Service.AddInitializedCallback(() =>
			{
				StartActivity(typeof(HomeActivity));
			});
		}
	}
}