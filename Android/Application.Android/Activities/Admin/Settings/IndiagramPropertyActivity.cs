#region Usings

using System;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using IndiaRose.Framework.Converters;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class IndiagramPropertyActivity : ActivityBase
	{
		protected override void OnCreate(Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Admin_Settings_IndiagramPropertyPage);
            SetViewModel(Container.Locator.AdminSettingsIndiagramPropertyViewModel);
		}
	}
}