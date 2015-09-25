using Android.App;
using Android.Content.PM;
using Android.OS;
using IndiaRose.Services.Android.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Inject;

namespace IndiaRose.Application.Activities.User
{
	[Activity(Label = "India Rose User Dev", Icon = "@drawable/ir_logo", Theme = "@style/Theme.Sherlock.Light.NoActionBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape, LaunchMode = LaunchMode.SingleInstance, NoHistory = true)]
	public class UserSplashscreenActivity : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.UserSplashScreen);

			LazyResolver<IInitializationStateService>.Service.AddInitializedCallback(() =>
			{
				StartActivity(typeof(UserHomeActivity));
			});
		}
	}
}