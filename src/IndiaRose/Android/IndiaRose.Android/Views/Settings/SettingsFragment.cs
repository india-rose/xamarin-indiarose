using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Support.Design.Widget;
using Android.Widget;
using IndiaRose.Core.Admins.ViewModels;
using ReactiveUI;

namespace IndiaRose.Droid.Views.Settings
{
	public class SettingsFragment : BaseFragment<SettingsViewModel>
	{
		private CoordinatorLayout _coordinatorLayout;
		private FloatingActionButton _saveButton;

		public SettingsFragment() : base(Resource.Layout.SettingsFragment)
		{
		}

		protected override void BindControls()
		{
			base.BindControls();
			ViewModel = new SettingsViewModel();

			_coordinatorLayout = RootView as CoordinatorLayout;
			_saveButton = RootView.FindViewById<FloatingActionButton>(Resource.Id.saveButton);

			ChildFragmentManager.BeginTransaction()
				.Add(Resource.Id.settings_preview, new SettingsPreviewFragment
				{
					ViewModel = ViewModel
				})
				.Add(Resource.Id.settings_display, new SettingsDisplayFragment
				{
					ViewModel = ViewModel
				})
				.Add(Resource.Id.settings_behavior, new SettingsBehaviorFragment
				{
					ViewModel = ViewModel
				})
				.Commit();

			this.WhenActivated(disposables =>
			{
				this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v._saveButton)
					.DisposeWith(disposables);

				ViewModel.SaveCommand
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(result =>
					{
						if (result)
						{
							Snackbar.Make(_coordinatorLayout, "Saved !", Snackbar.LengthShort).Show();
						}
						else
						{
							Snackbar.Make(_coordinatorLayout, "Error while saving !", Snackbar.LengthLong)
								.SetAction("Retry", v => ViewModel.SaveCommand.Execute().Subscribe())
								.Show();
						}
					}).DisposeWith(disposables);
			});
		}
	}
}