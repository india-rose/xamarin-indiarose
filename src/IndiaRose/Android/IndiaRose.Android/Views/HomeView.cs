using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Widget;
using IndiaRose.Core.Admins.ViewModels;
using IndiaRose.Droid.Views.Settings;
using ReactiveUI;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace IndiaRose.Droid.Views
{
	[Activity(MainLauncher = true, Theme = "@style/AppTheme", Label = "India Rose", Icon = "@mipmap/icon", ScreenOrientation = ScreenOrientation.Landscape)]
	public class HomeView : BaseActivity<HomeViewModel>
	{
		private Button NavigationButton { get; set; }

		public HomeView() : base(Resource.Layout.HomeView)
		{
		}

		protected override void BindControls()
		{
			base.BindControls();

			Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
#pragma warning disable 618
			Window.SetStatusBarColor(Resources.GetColor(Resource.Color.DarkPink));
#pragma warning restore 618
			toolbar.Title = "Test toolbar";

			ViewModel = new HomeViewModel();
			NavigationButton = FindViewById<Button>(Resource.Id.Button);

			FragmentManager.BeginTransaction()
				.Replace(Resource.Id.MenuFragmentContainer, new MenuFragment())
				.Commit();

			this.WhenActivated(() =>
			{
				CompositeDisposable disposables = new CompositeDisposable();

				this.BindCommand(ViewModel, vm => vm.OpenAppSettingsCommand, v => v.NavigationButton)
					.DisposeWith(disposables);

				ViewModel.OpenAppSettingsCommand
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(result =>
					{
						if (result)
						{
							StartActivity(typeof(SettingsView));
						}
					}).DisposeWith(disposables);

				return disposables;
			});
		}
	}
}