using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Storm.Mvvm;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IndiaRose.Application.Activities.Admin.Collection
{
    [Activity(Label = "CollectionManagementActivity")]
    public partial class CollectionManagementActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Admin_Collection_CollectionManagementActivity);
            SetViewModel(Container.Locator.AdminSettingsBackgroundColorViewModel);
        }
    }
}