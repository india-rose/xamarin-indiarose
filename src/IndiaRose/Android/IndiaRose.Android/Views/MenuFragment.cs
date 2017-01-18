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
using IndiaRose.Core.Admins.ViewModels;
using ReactiveUI;

namespace IndiaRose.Droid.Views
{
	public class MenuFragment : BaseFragment<MenuViewModel>
	{
		public MenuFragment() : base(Resource.Layout.MenuFragment)
		{
		}

		protected override void BindControls()
		{
			base.BindControls();

			Button settingsButton = RootView.FindViewById<Button>(Resource.Id.SettingsButton);
			Button collectionButton = RootView.FindViewById<Button>(Resource.Id.CollectionButton);

			collectionButton.Selected = true;
		}
	}
}