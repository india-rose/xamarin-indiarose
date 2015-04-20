using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using IndiaRose.Business.ViewModels.Admin.Collection;
using IndiaRose.Data.Model;
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

            Initialize();
        }

        private void Initialize()
        {
            WatchIndiagramViewModel vm = (WatchIndiagramViewModel)ViewModel;
            TextView nameTextView = FindViewById<TextView>(Resource.Id.text1);
            nameTextView.Text = vm.CurrentIndiagram.Text;
            TextView parentTextView = FindViewById<TextView>(Resource.Id.text2);
            Indiagram parent = vm.CurrentIndiagram.Parent;
            if (parent != null)
                parentTextView.Text = parent.Text;
            TextView soundpathTextView = FindViewById<TextView>(Resource.Id.text3);
            soundpathTextView.Text = vm.CurrentIndiagram.SoundPath ?? "Aucun";

        }
    }
}