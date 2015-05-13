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
    [Activity(Label = "India Rose User Dev", Icon = "@drawable/ir_logo", Theme = "@style/Theme.Sherlock.Light.NoActionBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape,
        LaunchMode = LaunchMode.SingleInstance)]
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
			base.OnCreate(bundle);
			SettingsService.Loaded += SettingsService_Loaded;
			lock (_mutex)
			{
				if (SettingsService.IsLoaded && !_initialized)
				{
					SettingsService.Loaded -= SettingsService_Loaded;
					_initialized = true;
					SetUp();
				}
			}
		}

		private void SettingsService_Loaded(object sender, EventArgs e)
		{
			lock (_mutex)
			{
				if (!_initialized)
				{
					SettingsService.Loaded -= SettingsService_Loaded;
					_initialized = true;
					SetUp();
				}
			}
		}

		private void SetUp()
		{
			SetContentView(Resource.Layout.User_HomePage);
		    SetViewModel(Container.Locator.UserHomeViewModel);

            //todo : enlever le layoutchange pour compatibilité avec ancienne version (cependant le layout n'est pas encore chargé à cet endroit donc que faire ?
            RootLayout.LayoutChange += OnLayoutChange;
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