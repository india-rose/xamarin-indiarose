using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IndiaRose.Android;

namespace IndiaRose.Droid.Views
{
	[Activity(MainLauncher = true, Name = "@strings/ApplicationName", Icon = "@drawable/Icon")]
	public class HomeActivity : BaseActivity
	{
		public HomeActivity() : base(Resource.Layout.HomeViewLayout)
		{
		}
	}
}