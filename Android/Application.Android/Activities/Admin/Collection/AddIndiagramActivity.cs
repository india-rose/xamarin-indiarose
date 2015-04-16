
using Android.OS;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.Admin.Collection
{
    public partial class AddIndiagramActivity : ActivityBase
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Admin_Collection_AddIndiagramPage);
            SetViewModel(Container.Locator.AdminCollectionAddIndiagramViewModel);
        }
    }
}