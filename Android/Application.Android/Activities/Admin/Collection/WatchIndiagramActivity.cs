using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
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
			
			WatchIndiagramViewModel vm = new WatchIndiagramViewModel();
			ImageView imageView = FindViewById<ImageView>(Resource.Id.image);
			if (vm.CurrentIndiagram != null && vm.CurrentIndiagram.ImagePath != null)
				imageView.SetImageBitmap(
					Bitmap.CreateScaledBitmap(
						BitmapFactory.DecodeFile(Environment.ExternalStorageDirectory.Path + "/IndiaRose/image/" + vm.CurrentIndiagram.ImagePath),
						imageView.Height, imageView.Width, true));
			else
				imageView.SetImageDrawable(new ColorDrawable(Color.Red));
				
            SetContentView(Resource.Layout.Admin_Collection_WatchIndiagramPage);
            SetViewModel(Container.Locator.AdminCollectionWatchIndiagramViewModel);

        }

        protected override void OnStart()
        {
            base.OnStart();
            Initialize();
        }

        private void Initialize()
        {
            WatchIndiagramViewModel vm = (WatchIndiagramViewModel)ViewModel;
            ViewGroup.LayoutParams indiagramParam = Watch_Img.LayoutParameters;
            indiagramParam.Height = vm.SettingsService.IndiagramDisplaySize;
            indiagramParam.Width = vm.SettingsService.IndiagramDisplaySize;
            Watch_Img.LayoutParameters = indiagramParam;
            TextView nameTextView = FindViewById<TextView>(Resource.Id.text1);
            nameTextView.Text = vm.CurrentIndiagram.Text;
            TextView parentTextView = FindViewById<TextView>(Resource.Id.text2);
            Indiagram parent = vm.CurrentIndiagram.Parent;
            if (parent != null)
                parentTextView.Text = parent.Text;
            TextView soundpathTextView = FindViewById<TextView>(Resource.Id.text3);
            soundpathTextView.Text = vm.CurrentIndiagram.SoundPath ?? soundpathTextView.Text;
            ImageView imageView = FindViewById<ImageView>(Resource.Id.Watch_Img);
            imageView.SetImageBitmap(Bitmap.CreateScaledBitmap(BitmapFactory.DecodeFile(Environment.ExternalStorageDirectory.Path+"/IndiaRose/image/"+vm.CurrentIndiagram.ImagePath),vm.SettingsService.IndiagramDisplaySize,vm.SettingsService.IndiagramDisplaySize,true));
           
            
        }
    }
}