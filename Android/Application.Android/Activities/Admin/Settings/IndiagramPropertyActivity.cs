#region Usings

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class IndiagramPropertyActivity : ActivityBase
	{
        [Binding("IndiagramSize")]
	    public int IndiaSize
	    {
            get { return Container.Locator.AdminSettingsIndiagramPropertyViewModel.IndiagramSize; }
            set { RefreshAreasSize(value);}
	    }
		protected override void OnCreate(Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Admin_Settings_IndiagramPropertyPage);
            SetViewModel(Container.Locator.AdminSettingsIndiagramPropertyViewModel);

            RefreshAreasSize(Container.Locator.AdminSettingsIndiagramPropertyViewModel.IndiagramSize);
		}

        private void RefreshAreasSize(int size)
	    {
	        ViewGroup.LayoutParams square = Square.LayoutParameters;
	        square.Height = size;
	        square.Width = size;

            Square.LayoutParameters = square;
            Container.Locator.AdminSettingsIndiagramPropertyViewModel.IndiagramSize = size;
	    }
	}
}