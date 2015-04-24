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
        private string _text;
        private Indiagram _parent;
        private string _imagePath;
        private string _soundPath;
        private int _position;

        [Binding("CurrentIndiagram.Text")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (SetProperty(ref _text, value))
                {
                    RefreshView("text");
                }
            }
        }
        [Binding("CurrentIndiagram.Parent")]
        public Indiagram ParentIndiagram
        {
            get { return _parent; }
            set { if(SetProperty(ref _parent, value)){
                    RefreshView("parent");
                }}
        }
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
        [Binding("CurrentIndiagram.SoundPath")]
        public string SoundPath
        {
            get { return _soundPath; }
            set
            {
                if(SetProperty(ref _soundPath, value))
                {
                    RefreshView("soundpath");
                }
            }
        }

        public int Position
        {
            get { return _position; }
            set
            {
                if(SetProperty(ref _position, value))
                {
                    RefreshView("position");
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
                case "text":
                    EditText nameEditText = FindViewById<EditText>(Resource.Id.edit_text);
                    nameEditText.Text = Text;
                    break;
                case "imagepath":
                    ImageView imageView = FindViewById<ImageView>(Resource.Id.Add_Img);
                    if (ImagePath != null)
                    {
                        AddIndiagramViewModel vm = (AddIndiagramViewModel)ViewModel;
                        var size = vm.SettingsService.IndiagramDisplaySize;
                        imageView.SetImageBitmap(Bitmap.CreateScaledBitmap(BitmapFactory.DecodeFile(ImagePath),size,size,true));

                    }
                    break;
                case "soundpath":
                    TextView soundpathTextView = FindViewById<TextView>(Resource.Id.m_indiagramSound);
                    if (SoundPath != null)
                        soundpathTextView.Text = SoundPath;
                    else
                    {
                        var trad = DependencyService.Container.Resolve<ILocalizationService>();
                        soundpathTextView.Text = trad.GetString("AIP_NoSound", "Text");
                    }
                    break;
                case "parent":
                    TextView parentTextView = FindViewById<TextView>(Resource.Id.m_indiagramCategory);
                    Indiagram parent = ParentIndiagram;
                    if (parent != null)
                        parentTextView.Text = parent.Text;
                    else
                    {
                        var trad = DependencyService.Container.Resolve<ILocalizationService>();
                        parentTextView.Text = trad.GetString("Root_Categ", "Text");
                    }
                    break;
                case "position":
                    break;
                case "all":
                    RefreshView("text");
                    RefreshView("imagepath");
                    RefreshView("soundpath");
                    RefreshView("parent");
                    RefreshView("position");
                    break;
            }
        }
    }
}