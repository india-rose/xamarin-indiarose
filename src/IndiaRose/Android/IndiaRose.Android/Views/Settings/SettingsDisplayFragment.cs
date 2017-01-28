using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Core.Admins.ViewModels;
using IndiaRose.Droid.Controls;
using ReactiveUI;

namespace IndiaRose.Droid.Views.Settings
{
	public class SettingsDisplayFragment : BaseFragment<SettingsViewModel>
	{
		private ColorPickerView _colorPickerView;
		private Button _changeTopColorButton;
		private Button _changeBottomColorButton;

		public SettingsDisplayFragment() : base(Resource.Layout.SettingsDisplayFragment)
		{
			
		}

		protected override SettingsViewModel CreateViewModel()
		{
			return null; //on purpose, will be affected from parent
		}

		protected override void BindControls()
		{
			base.BindControls();

			_changeTopColorButton = RootView.FindViewById<Button>(Resource.Id.ChangeTopColorButton);
			_changeBottomColorButton = RootView.FindViewById<Button>(Resource.Id.ChangeBottomColorButton);
			_colorPickerView = RootView.FindViewById<ColorPickerView>(Resource.Id.ColorPickerView);

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel, vm => vm.IsTopColorChanging, v => v._changeTopColorButton.Selected).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.IsBottomColorChanging, v => v._changeBottomColorButton.Selected).DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.ChangeTopColor, v => v._changeTopColorButton).DisposeWith(disposables);
				this.BindCommand(ViewModel, vm => vm.ChangeBottomColor, v => v._changeBottomColorButton).DisposeWith(disposables);

				//color picker is only visible if one of the button is in selected state
				this.WhenAnyValue(v => v.ViewModel.IsTopColorChanging, v => v.ViewModel.IsBottomColorChanging, (x, y) => x || y)
					.Select(selected => selected ? ViewStates.Visible : ViewStates.Gone)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(visibility => _colorPickerView.Visibility = visibility)
					.DisposeWith(disposables);
			});
		}
	}
}