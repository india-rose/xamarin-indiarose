using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Core.Admins.ViewModels;
using IndiaRose.Droid.Views.Settings;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace IndiaRose.Droid.Views
{
	[Activity(MainLauncher = true, Theme = "@style/AppTheme", Label = "India Rose", Icon = "@mipmap/icon")]
	public class HomeView : BaseActivity<HomeViewModel>
	{
		public HomeView() : base(Resource.Layout.HomeView)
		{
		}
		
		protected override void BindControls()
		{
			base.BindControls();

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			}
			else
			{
				Window.AddFlags(WindowManagerFlags.TranslucentStatus);
			}

			Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
			Title = "Test toolbar 2";
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);

			ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
			viewPager.Adapter = new ViewPagerAdapter(SupportFragmentManager)
			{
				{LocalizedStrings.Menu_Settings, new SettingsFragment()},
				{"Collection", new MenuFragment()},
				{"About", new MenuFragment()},
				{"Credits", new MenuFragment()},
				{"Contact", new MenuFragment()},
			};
			TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);
			tabs.SetupWithViewPager(viewPager);
		}

		class ViewPagerAdapter : FragmentPagerAdapter, IEnumerable
		{
			private readonly List<Tuple<string, Fragment>> _fragments = new List<Tuple<string, Fragment>>();
			
			public ViewPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
			{
			}

			public ViewPagerAdapter(FragmentManager fm) : base(fm)
			{
			}

			public override int Count => _fragments.Count;
			public override Fragment GetItem(int position)
			{
				return _fragments[position].Item2;
			}

			public override ICharSequence GetPageTitleFormatted(int position)
			{
				return new Java.Lang.String(_fragments[position].Item1);
			}

			public void Add(string title, Fragment fragment)
			{
				_fragments.Add(Tuple.Create(title, fragment));
			}

			public IEnumerator GetEnumerator()
			{
				return _fragments.GetEnumerator();
			}
		}
	}
}