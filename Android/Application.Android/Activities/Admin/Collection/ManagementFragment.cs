using Android.Views;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.Admin.Collection
{
    public partial class ManagementFragment : FragmentBase
    {
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {/*
            ManagementViewModel management = (ManagementViewModel) ViewModel;
            List<Indiagram> indigramesList = GetTopLevel();
            ImageView image 
            image.SetImageBitmap(Bitmap.CreateScaledBitmap(BitmapFactory.DecodeFile(indi.ImagePath), management.SettingsService.IndiagramDisplaySize, management.SettingsService.IndiagramDisplaySize, true));
            var imageView = FindViewById<ImageView>(Resource.Id.Watch_Img); */
            return inflater.Inflate(Resource.Layout.Views_Admin_Collection_ManagementPage, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminManagementViewModel;
        }
    }
}