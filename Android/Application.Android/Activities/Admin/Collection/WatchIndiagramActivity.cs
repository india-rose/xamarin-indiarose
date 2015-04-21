using Android.App;
using Android.Content.PM;
using Android.Graphics;
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

            SetContentView(Resource.Layout.Admin_Collection_WatchIndiagramPage);
            SetViewModel(Container.Locator.AdminCollectionWatchIndiagramViewModel);

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
            soundpathTextView.Text = vm.CurrentIndiagram.SoundPath ?? "Aucun";
            ImageView imageView = FindViewById<ImageView>(Resource.Id.Watch_Img);
            //TODO linké l'image avec l'image de l'indiagram
            /*
             * https://github.com/Julien-Mialon/IndiaRose/blob/master/JavaVersion/EclipseWS/IndiaRoseLibrary/src/org/indiarose/lib/utils/ImageManager.java
             * https://github.com/Julien-Mialon/IndiaRose/blob/master/JavaVersion/EclipseWS/IndiaRoseLibrary/src/org/indiarose/lib/PathData.java
             * try
		{
			m_indiagramImage.setImageBitmap(ImageManager.loadImage(PathData.IMAGE_DIRECTORY + m_indiagram.imagePath, AppData.settings.indiagramSize, AppData.settings.indiagramSize));
		} 
		catch (Exception e)
		{
			//Log.wtf("AddIndiagram", "Unable to load image", e);
		}
             */
            
        }
    }
}