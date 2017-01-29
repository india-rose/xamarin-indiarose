using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Views;
using Android.Widget;
using IndiaRose.Core.Admins.ViewModels;
using IndiaRose.Droid.Controls;
using IndiaRose.Droid.Extensions;
using ReactiveUI;

namespace IndiaRose.Droid.Views.Settings
{
	public class SettingsDisplayFragment : BaseFragment<SettingsViewModel>
	{
		private ColorPickerView _colorPickerView;
		private Button _changeTopColorButton;
		private Button _changeBottomColorButton;

		private TextView _indiagramSizeLabel;
		private SeekBar _indiagramSizePicker;

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
			_colorPickerView = RootView.FindViewById<ColorPickerView>(Resource.Id.BackgroundColorPickerView);

			_indiagramSizeLabel = RootView.FindViewById<TextView>(Resource.Id.IndiagramSizeValueLabel);
			_indiagramSizePicker = RootView.FindViewById<SeekBar>(Resource.Id.IndiagramSizeValuePicker);

			this.WhenActivated(disposables =>
			{
				#region Background color + indiagram size

				this.OneWayBind(ViewModel, vm => vm.IsTopColorChanging, v => v._changeTopColorButton.Selected).DisposeWith(disposables);
				this.OneWayBind(ViewModel, vm => vm.IsBottomColorChanging, v => v._changeBottomColorButton.Selected).DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.ChangeTopBackgroundColor, v => v._changeTopColorButton).DisposeWith(disposables);
				this.BindCommand(ViewModel, vm => vm.ChangeBottomBackgroundColor, v => v._changeBottomColorButton).DisposeWith(disposables);

				//color picker is only visible if one of the button is in selected state
				this.WhenAnyValue(v => v.ViewModel.IsTopColorChanging, v => v.ViewModel.IsBottomColorChanging, (x, y) => x || y)
					.Select(selected => selected ? ViewStates.Visible : ViewStates.Gone)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(visibility => _colorPickerView.Visibility = visibility)
					.DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.IsTopColorChanging)
					.Where(x => x)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(_ => _colorPickerView.SetColor(ViewModel.TopColor.ToIntColor()))
					.DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.IsBottomColorChanging)
					.Where(x => x)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(_ => _colorPickerView.SetColor(ViewModel.BottomColor.ToIntColor()))
					.DisposeWith(disposables);

				IObservable<EventPattern<int>> backgroundColorChangedObservable = Observable.FromEventPattern<EventHandler<int>, int>(handler => _colorPickerView.OnColorChanged += handler, handler => _colorPickerView.OnColorChanged -= handler);
				backgroundColorChangedObservable.Select(color => color.EventArgs.StringFromColor())
					.Subscribe(color => ViewModel.UpdateBackgroundColor.Execute(color))
					.DisposeWith(disposables);


				this.OneWayBind(ViewModel, vm => vm.IndiagramSize, v => v._indiagramSizeLabel.Text).DisposeWith(disposables);
				_indiagramSizePicker.Progress = ViewModel?.IndiagramSizePercentage ?? 0;
				var indiagramSizeChangedObservable = Observable.FromEventPattern<EventHandler<SeekBar.ProgressChangedEventArgs>, SeekBar.ProgressChangedEventArgs>(handler => _indiagramSizePicker.ProgressChanged += handler, handler => _indiagramSizePicker.ProgressChanged -= handler);
				indiagramSizeChangedObservable.Select(args => args.EventArgs.Progress)
					.Subscribe(sizePercentage => ViewModel.UpdateIndiagramSize.Execute(sizePercentage))
					.DisposeWith(disposables);

				#endregion


			});
		}
	}
}