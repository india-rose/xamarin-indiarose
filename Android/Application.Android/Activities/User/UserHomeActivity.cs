using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using IndiaRose.Framework.Views;
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
			LazyResolver<ILoggerService>.Service.Log("OnCreate UserHomeActivity", MessageSeverity.Error);
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.User_HomePage);
			SetViewModel(Container.Locator.UserHomeViewModel);

			//TODO : regarder du cote de ViewTreeObserver

			//todo : enlever le layoutchange pour compatibilité avec ancienne version 
			//(cependant le layout n'est pas encore chargé à cet endroit donc que faire ?
		    //RelativeLayout RootLayout = FindViewById<RelativeLayout>(Resource.Id.RootLayout);
			//RootLayout.ViewTreeObserver.

			RootLayout.LayoutChange += OnLayoutChange;
		}

	    protected override void OnDestroy()
	    {
			LazyResolver<ITextToSpeechService>.Service.Close();
		    base.OnDestroy();
	    }
        private void OnLayoutChange(object sender, View.LayoutChangeEventArgs layoutChangeEventArgs)
        {
            if (RootLayout.Height > 0)
            {
                RootLayout.LayoutChange -= OnLayoutChange;
                Initialize();
            }
        }

        private void Initialize()
        {
            int availableHeight = RootLayout.Height - TitleBar.Height;
	        MiddleScreen.Init(availableHeight, RootLayout.Width);

			//UserView mid = FindViewById<UserView>(Resource.Id.MiddleScreen);
			//mid.Init(availableHeight,RootLayout.Width);
        }
	}
}