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
	public partial class BackgroundColorActivity : ActivityBase
	{
	    private int _backgroundTopPercentSize;

        [Binding("CurrentSize")]
	    public int BackgroundTopPercentSize
	    {
            get { return _backgroundTopPercentSize; }
	        set
	        {
	            if (SetProperty(ref _backgroundTopPercentSize, value))
	            {
	                RefreshAreasSize();
	            }
	        }
	    }

	    private void RefreshAreasSize()
	    {
	        int height = RightLayout.Height;
            int topHeight = (int)(height * (BackgroundTopPercentSize / 100.0));
            int bottomHeight = height - topHeight;
	        
            ViewGroup.LayoutParams top = TopArea.LayoutParameters;
            top.Height = topHeight;
	        TopArea.LayoutParameters = top;
            ViewGroup.LayoutParams bottom = BottomArea.LayoutParameters;
            bottom.Height = bottomHeight;
	        BottomArea.LayoutParameters = bottom;
	    }

	    protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Admin_Settings_BackgroundColorPage);
			SetViewModel(Container.Locator.AdminSettingsBackgroundColorViewModel);


		    RightLayout.LayoutChange += OnLayoutInitialized;
		}

		// ReSharper disable UnusedParameter.Local cause of implicit creation of vars
		private void OnLayoutInitialized(object sender, View.LayoutChangeEventArgs layoutChangeEventArgs)
		// ReSharper restore UnusedParameter.Local
		{
			if (RightLayout.Height > 0)
			{
				RightLayout.LayoutChange -= OnLayoutInitialized;
				RefreshAreasSize();
			}
		}
	}
}