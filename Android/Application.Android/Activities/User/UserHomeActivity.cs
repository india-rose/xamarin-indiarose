using Android.App;
using Android.Content.PM;
using Android.OS;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using IndiaRose.Framework.Extensions;

namespace IndiaRose.Application.Activities.User
{
    [Activity(Theme = "@style/Theme.Sherlock.Light.NoActionBar", ScreenOrientation = ScreenOrientation.Landscape)]
	public partial class UserHomeActivity : ActivityBase
	{
	    protected override void OnCreate(Bundle bundle)
		{
			LazyResolver<ILoggerService>.Service.Log("OnCreate UserHomeActivity", MessageSeverity.Error);
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.User_HomePage);
			SetViewModel(Container.Locator.UserHomeViewModel);

			RootLayout.AttachOnLayoutChanged(Initialize);
		}

	    protected override void OnDestroy()
	    {
			LazyResolver<ITextToSpeechService>.Service.Close();
		    base.OnDestroy();
	    }

        private void Initialize()
        {
            int availableHeight = RootLayout.Height - TitleBar.Height;
	        MiddleScreen.Init(availableHeight, RootLayout.Width);
        }
	}
}