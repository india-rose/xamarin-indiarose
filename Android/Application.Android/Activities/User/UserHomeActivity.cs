using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.User
{
	[Activity(Theme = "@style/Theme.Sherlock.Light.NoActionBar", ScreenOrientation = ScreenOrientation.Landscape)]
	public partial class UserHomeActivity : ActivityBase
	{
		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.NoTitle);
			Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
			Window.AddFlags(WindowManagerFlags.KeepScreenOn);

			LazyResolver<ILoggerService>.Service.Log("OnCreate UserHomeActivity", MessageSeverity.Error);
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.User_HomePage);
			SetViewModel(Container.Locator.UserHomeViewModel);

			DisplayMetrics windowSize = new DisplayMetrics();
			WindowManager.DefaultDisplay.GetMetrics(windowSize);

			int availableHeight = windowSize.HeightPixels - 60;
			MiddleScreen.Init(availableHeight, windowSize.WidthPixels);
		}

		protected override void OnDestroy()
		{
			LazyResolver<ITextToSpeechService>.Service.Close();
			base.OnDestroy();
		}
	}
}