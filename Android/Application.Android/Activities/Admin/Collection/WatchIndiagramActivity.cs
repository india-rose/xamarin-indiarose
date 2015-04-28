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
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

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
		}
	}
}