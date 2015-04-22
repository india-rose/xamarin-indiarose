#region Usings

using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using IndiaRose.Framework.Converters;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class IndiagramPropertyActivity : ActivityBase
	{
		private bool _allowUpdate = false;
		private int _indiagramSize;
		private bool _reinforcerEnabled;
		private Color _reinforcerColor;

        [Binding("IndiagramSize")]
	    public int IndiagramSize
	    {
            get { return _indiagramSize; }
	        set
	        {
		        if (SetProperty(ref _indiagramSize, value) && value > 0)
		        {
			        RefreshPreview();
		        }
	        }
	    }

		[Binding("ReinforcerColor.Color", Converter = typeof(ColorStringToColor))]
		public Color ReinforcerColor
		{
			get { return _reinforcerColor; }
			set 
			{
				if (SetProperty(ref _reinforcerColor, value))
				{
					RefreshPreview();
				}
			}
		}

		[Binding("ReinforcerEnabled")]
		public bool ReinforcerEnabled
		{
			get { return _reinforcerEnabled; }
			set
			{
				if (SetProperty(ref _reinforcerEnabled, value))
				{
					RefreshPreview();
				}
			}
		}

		protected override void OnCreate(Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Admin_Settings_IndiagramPropertyPage);
            SetViewModel(Container.Locator.AdminSettingsIndiagramPropertyViewModel);

			PreviewContainer.LayoutChange += OnLayoutInitialized;
		}

		// ReSharper disable UnusedParameter.Local cause of implicit creation of vars
		private void OnLayoutInitialized(object sender, View.LayoutChangeEventArgs layoutChangeEventArgs)
		// ReSharper restore UnusedParameter.Local
		{
			PreviewContainer.LayoutChange -= OnLayoutInitialized;
			_allowUpdate = true;
			RefreshPreview();
		}

        private void RefreshPreview()
        {
	        if (!_allowUpdate)
	        {
		        return;
	        }

	        ViewGroup.LayoutParams indiagramParam = IndiagramArea.LayoutParameters;
	        ViewGroup.LayoutParams reinforcerParam = ReinforcerArea.LayoutParameters;

			indiagramParam.Height = IndiagramSize;
			indiagramParam.Width = IndiagramSize;

			reinforcerParam.Height = (int)(IndiagramSize * 1.2);
			reinforcerParam.Width = (int)(IndiagramSize * 1.2);

			ReinforcerArea.LayoutParameters = reinforcerParam;
			IndiagramArea.LayoutParameters = indiagramParam;

			ReinforcerArea.SetBackgroundColor(ReinforcerEnabled ? ReinforcerColor : Color.Transparent);
			IndiagramArea.SetBackgroundColor(Color.Red);
		}
	}

    //TODO COULEUR TEXTE
}