using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using IndiaRose.Interfaces;
using IndiaRose.Services.Android;
using Storm.Mvvm;
using Storm.Mvvm.Inject;

namespace IndiaRose.Application.Activities.User
{
    //[Activity(Label = "India Rose User Dev", Icon = "@drawable/ir_logo", Theme = "@style/Theme.Sherlock.Light.NoActionBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape, LaunchMode = LaunchMode.SingleTask)]
    public partial class UserHomeActivity : ActivityBase
    {
        protected ISettingsService SettingsService { get { return LazyResolver<ISettingsService>.Service; } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SettingsService.Loaded+=SettingsService_Loaded;
            SetViewModel(Container.Locator.UserHomeViewModel);
        }

        private void SettingsService_Loaded(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.User_HomePage);
            Initialize();
        }

        private void Initialize()
        {

            int height = title_bar.LayoutParameters.Height;

            ViewGroup.LayoutParams top = top_indiabrowser.LayoutParameters;
            top.Height = height * SettingsService.SelectionAreaHeight;
            top_indiabrowser.LayoutParameters = top;
            ViewGroup.LayoutParams bottom = bot_sentencearea.LayoutParameters;
            bottom.Height = height * (1 - SettingsService.SelectionAreaHeight);
            bot_sentencearea.LayoutParameters = bottom;
        }
    }
}