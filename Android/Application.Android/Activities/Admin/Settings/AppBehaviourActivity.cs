using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.Admin.Settings
{
    [Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar", Icon = "@drawable/ir_logo_params")]
    public partial class AppBehaviourActivity : ActivityBase
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Admin_Settings_AppBehaviourPage);
            SetViewModel(Container.Locator.AdminSettingsAppBehaviourViewModel);
        }
    }
}