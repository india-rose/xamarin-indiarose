
using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using IndiaRose.Business.ViewModels.Admin.Collection;
using IndiaRose.Data.Model;
using IndiaRose.Services;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.Admin.Collection
{

    [Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
    public partial class AddIndiagramActivity : ActivityBase
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Admin_Collection_AddIndiagramPage);
            SetViewModel(Container.Locator.AdminCollectionAddIndiagramViewModel);

            Initialize();
        }

        private void Initialize()
        {
            AddIndiagramViewModel vm = (AddIndiagramViewModel)ViewModel;

            ViewGroup.LayoutParams indiagramParam = Add_Img.LayoutParameters;
            indiagramParam.Height = vm.SettingsService.IndiagramDisplaySize;
            indiagramParam.Width = vm.SettingsService.IndiagramDisplaySize;
            Add_Img.LayoutParameters = indiagramParam;

            if (vm.CurrentIndiagram == null)
                return;
            EditText nameEditText = FindViewById<EditText>(Resource.Id.edit_text);
            nameEditText.Text = vm.CurrentIndiagram.Text;
            TextView parentTextView = FindViewById<TextView>(Resource.Id.m_indiagramCategory);
            Indiagram parent = vm.CurrentIndiagram.Parent;
            if (parent != null)
                parentTextView.Text = parent.Text;
            TextView soundpathTextView = FindViewById<TextView>(Resource.Id.m_indiagramSound);
            soundpathTextView.Text = vm.CurrentIndiagram.SoundPath ?? soundpathTextView.Text;

        }
    }
}