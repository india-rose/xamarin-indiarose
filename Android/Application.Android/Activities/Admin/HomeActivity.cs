#region Usings

using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Application.Activities.Admin
{
    [Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class HomeActivity : ActivityBase
	{
		protected override void OnCreate(Bundle bundle)
		{
			LazyResolver<ILoggerService>.Service.Log("OnCreate HomeActivity", MessageSeverity.Error); 
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Admin_HomePage);
			SetViewModel(Container.Locator.AdminHomeViewModel);
		}

	    protected override void OnStop()
	    {
            LazyResolver<ITextToSpeechService>.Service.Close();
	        base.OnStop();
	    }
	}
}