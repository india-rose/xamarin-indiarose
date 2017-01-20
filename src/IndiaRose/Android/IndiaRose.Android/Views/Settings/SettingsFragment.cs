using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.OS;
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

		protected override void CreateFragments()
		{
			base.CreateFragments();
			ChildFragmentManager.BeginTransaction()
				.Replace(Resource.Id.settings_preview, new SettingsPreviewFragment
				{
					ViewModel = ViewModel
				}, nameof(SettingsPreviewFragment))
				.Replace(Resource.Id.settings_display, new SettingsDisplayFragment
				{
					ViewModel = ViewModel
				}, nameof(SettingsDisplayFragment))
				.Replace(Resource.Id.settings_behavior, new SettingsBehaviorFragment
				{
					ViewModel = ViewModel
				}, nameof(SettingsBehaviorFragment))
				.Commit();
		}

		protected override void RestoreFragments()
		{
			base.RestoreFragments();

			SettingsPreviewFragment preview = ChildFragmentManager.FindFragmentByTag(nameof(SettingsPreviewFragment)) as SettingsPreviewFragment;
			if (preview != null)
			{
				preview.ViewModel = ViewModel;
			}

			SettingsDisplayFragment display = ChildFragmentManager.FindFragmentByTag(nameof(SettingsDisplayFragment)) as SettingsDisplayFragment;
			if (display != null)
			{
				display.ViewModel = ViewModel;
			}

			SettingsBehaviorFragment behavior = ChildFragmentManager.FindFragmentByTag(nameof(SettingsBehaviorFragment)) as SettingsBehaviorFragment;
			if (behavior != null)
			{
				behavior.ViewModel = ViewModel;
			}
		}

		protected override void BindControls()
		{
			base.BindControls();

			_coordinatorLayout = RootView as CoordinatorLayout;
			_saveButton = RootView.FindViewById<FloatingActionButton>(Resource.Id.saveButton);

			this.WhenActivated(disposables =>
			{

				this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v._saveButton)
					.DisposeWith(disposables);

				this.WhenAnyObservable(x => x.ViewModel.SaveCommand)
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

		protected override SettingsViewModel CreateViewModel()
		{
			return new SettingsViewModel();
		}
	}
}