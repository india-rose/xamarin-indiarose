
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using IndiaRose.Data.Model;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;

namespace IndiaRose.Application.Activities.Admin.Collection
{
    [Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
    public partial class CollectionManagementActivity : ActivityBase
    {
		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Admin_Collection_CollectionManagementPage);
            SetViewModel(Container.Locator.AdminCollectionManagementViewModel);
        }
    }
}