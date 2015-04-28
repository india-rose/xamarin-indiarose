using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using IndiaRose.Business.ViewModels.Admin.Collection;
using IndiaRose.Data.Model;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection
{

    [Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
    public partial class AddIndiagramActivity : ActivityBase
    {
        #region Properties
        private string _imagePath;
        private bool _isEnable;

        [Binding("CurrentIndiagram.ImagePath")]
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if(SetProperty(ref _imagePath, value))
                {
                    RefreshView("imagepath");
                }
            }
        }
        [Binding("CurrentIndiagram.IsEnabled")]
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (SetProperty(ref _isEnable, value))
                {
                    RefreshView("isEnable");
                }
            }
        }
        #endregion
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Admin_Collection_AddIndiagramPage);
            SetViewModel(Container.Locator.AdminCollectionAddIndiagramViewModel);

        }

        protected override void OnStart()
        {
            base.OnStart();
            Initialize();
        }

        private void Initialize()
        {
            AddIndiagramViewModel vm = (AddIndiagramViewModel)ViewModel;

            ViewGroup.LayoutParams indiagramParam = Add_Img.LayoutParameters;
            indiagramParam.Height = vm.SettingsService.IndiagramDisplaySize;
            indiagramParam.Width = vm.SettingsService.IndiagramDisplaySize;
            Add_Img.LayoutParameters = indiagramParam;
            RefreshView("all");

        }

        private void RefreshView(string champ)
        {
            switch (champ)
            {
                case "imagepath":
                    ImageView imageView = FindViewById<ImageView>(Resource.Id.Add_Img);
                    if (ImagePath != null)
                    {
                        AddIndiagramViewModel vm = (AddIndiagramViewModel)ViewModel;
                        var size = vm.SettingsService.IndiagramDisplaySize;
                        imageView.SetImageBitmap(Bitmap.CreateScaledBitmap(BitmapFactory.DecodeFile(ImagePath),size,size,true));
                    }
                    break;
                case "isEnable":
                    Button button;
                    if (IsEnable)
                    {
                        button = FindViewById<Button>(Resource.Id.act);
                        button.Visibility = ViewStates.Gone;
                        button = FindViewById<Button>(Resource.Id.desact);
                        button.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        button = FindViewById<Button>(Resource.Id.act);
                        button.Visibility = ViewStates.Visible;
                        button = FindViewById<Button>(Resource.Id.desact);
                        button.Visibility = ViewStates.Gone;
                    }
                    break;
                case "all":
                    LinearLayout focusLinearLayout = FindViewById<LinearLayout>(Resource.Id.focus);
                    focusLinearLayout.RequestFocus();
                    RefreshView("imagepath");
                    RefreshView("isEnable");
                    break;
            }
        }
    }
}