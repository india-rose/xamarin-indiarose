using System;
using Android.App;
using Android.Runtime;

namespace IndiaRose.Droid
{
	[Application]
	public class IndiaRoseApplication : Application
	{
		public IndiaRoseApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			LocalizedStrings.Initialize(this);
		}
	}
}