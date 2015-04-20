using Android.App;
using Android.Content.PM;
using Android.OS;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.Admin.Collection
{
     [Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
    public partial class WatchIndiagramActivity : ActivityBase
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Admin_Collection_WatchIndiagramPage);
            SetViewModel(Container.Locator.AdminCollectionWatchIndiagramViewModel);
        }
    }
}