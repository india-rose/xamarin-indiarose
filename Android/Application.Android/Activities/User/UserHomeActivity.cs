  using Android.App;
  using Android.Content.PM;
  using Android.OS;
  using Storm.Mvvm;

namespace IndiaRose.Application.Activities.User
{
    [Activity(Label = "India Rose User Dev", Icon = "@drawable/ir_logo", Theme = "@style/Theme.Sherlock.Light.NoActionBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public partial class UserHomeActivity : ActivityBase
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.User_HomePage);
            SetViewModel(Container.Locator.UserHomeViewModel);
        }
    }
}