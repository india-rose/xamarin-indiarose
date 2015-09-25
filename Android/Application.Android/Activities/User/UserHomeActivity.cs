using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
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
        public ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }
		private bool _initialized;
		private readonly object _mutex = new object();

		protected override void OnCreate(Bundle bundle)
		{
			LazyResolver<ILoggerService>.Service.Log("OnCreate UserHomeActivity", MessageSeverity.Error);
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.User_HomePage);
			SetViewModel(Container.Locator.UserHomeViewModel);

			//todo : enlever le layoutchange pour compatibilité avec ancienne version (cependant le layout n'est pas encore chargé à cet endroit donc que faire ?
			RootLayout.LayoutChange += OnLayoutChange;
		}

	    protected override void OnStop()
	    {
			LazyResolver<ITextToSpeechService>.Service.Close();
		    base.OnStop();
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
            UserView mid = FindViewById<UserView>(Resource.Id.MiddleScreen);
            mid.Init(availableHeight,RootLayout.Width);
        }
	}
}