using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Graphics;
using Android.Util;
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
		private ColorPickerView _backgroundColorPicker;
		private Button _changeTopColorButton;
		private Button _changeBottomColorButton;

		private TextView _indiagramSizeLabel;
		private SeekBar _indiagramSizePicker;

		private Spinner _fontFamilyPicker;

		private TextView _fontSizeLabel;
		private SeekBar _fontSizePicker;

		private TextView _fontPreviewTop;
		private TextView _fontPreviewBottom;

		private Button _changeTextColorButton;
		private ColorPickerView _textColorPicker;

		private CheckBox _isReinforcerEnabledCheckBox;
		private Button _changeReinforcerColorButton;
		private ColorPickerView _reinforcerColorPicker;

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
			_backgroundColorPicker = RootView.FindViewById<ColorPickerView>(Resource.Id.BackgroundColorPickerView);

			_indiagramSizeLabel = RootView.FindViewById<TextView>(Resource.Id.IndiagramSizeValueLabel);
			_indiagramSizePicker = RootView.FindViewById<SeekBar>(Resource.Id.IndiagramSizeValuePicker);

			_fontFamilyPicker = RootView.FindViewById<Spinner>(Resource.Id.FontFamilyPicker);

			_fontSizeLabel = RootView.FindViewById<TextView>(Resource.Id.FontSizeValueLabel);
			_fontSizePicker = RootView.FindViewById<SeekBar>(Resource.Id.FontSizeValuePicker);

			_fontPreviewTop = RootView.FindViewById<TextView>(Resource.Id.FontPreviewTop);
			_fontPreviewBottom = RootView.FindViewById<TextView>(Resource.Id.FontPreviewBottom);

			_textColorPicker = RootView.FindViewById<ColorPickerView>(Resource.Id.TextColorPickerView);
			_changeTextColorButton = RootView.FindViewById<Button>(Resource.Id.ChangeTextColorButton);

			_isReinforcerEnabledCheckBox = RootView.FindViewById<CheckBox>(Resource.Id.EnableReinforcerCheckbox);
			_changeReinforcerColorButton = RootView.FindViewById<Button>(Resource.Id.ChangeReinforcerColorButton);
			_reinforcerColorPicker = RootView.FindViewById<ColorPickerView>(Resource.Id.ReinforcerColorPickerView);

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
					.Subscribe(visibility => _backgroundColorPicker.Visibility = visibility)
					.DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.IsTopColorChanging)
					.Where(x => x)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(_ => _backgroundColorPicker.SetColor(ViewModel.TopColor.ToIntColor()))
					.DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.IsBottomColorChanging)
					.Where(x => x)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(_ => _backgroundColorPicker.SetColor(ViewModel.BottomColor.ToIntColor()))
					.DisposeWith(disposables);

				IObservable<EventPattern<int>> backgroundColorChangedObservable = Observable.FromEventPattern<EventHandler<int>, int>(handler => _backgroundColorPicker.OnColorChanged += handler, handler => _backgroundColorPicker.OnColorChanged -= handler);
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

				#region Fonts
				//family
				this.WhenAnyValue(v => v.ViewModel.FontNames)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(fonts =>
					{
						if (fonts != null)
						{
							_fontFamilyPicker.Adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItem1, fonts);
						}
					})
					.DisposeWith(disposables);
				
				this.WhenAnyValue(v => v.ViewModel.SelectedFont, v => v.ViewModel.FontNames)
					.Where(x => x.Item1 != null && x.Item2 != null)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(fontInfo =>
					{
						int index = fontInfo.Item2.FindIndex(x => x.Equals(fontInfo.Item1, StringComparison.OrdinalIgnoreCase));
						if (index >= 0)
						{
							_fontFamilyPicker.SetSelection(index);
						}
					}).DisposeWith(disposables);

				IObservable<EventPattern<AdapterView.ItemSelectedEventArgs>> selectedFontChangedObservable = Observable.FromEventPattern<AdapterView.ItemSelectedEventArgs>(handler => _fontFamilyPicker.ItemSelected += handler, handler => _fontFamilyPicker.ItemSelected -= handler);
				selectedFontChangedObservable.Select(x => x.EventArgs.Position)
					.Subscribe(fontIndex => ViewModel?.SelectFont?.Execute(fontIndex))
					.DisposeWith(disposables);

				//size
				this.OneWayBind(ViewModel, vm => vm.FontSize, v => v._fontSizeLabel.Text).DisposeWith(disposables);
				_fontSizePicker.Progress = ViewModel?.FontSize ?? 0;
				var fontSizeChangedObservable = Observable.FromEventPattern<EventHandler<SeekBar.ProgressChangedEventArgs>, SeekBar.ProgressChangedEventArgs>(handler => _fontSizePicker.ProgressChanged += handler, handler => _fontSizePicker.ProgressChanged -= handler);
				fontSizeChangedObservable.Select(args => args.EventArgs.Progress)
					.Subscribe(fontSize => ViewModel.UpdateFontSize.Execute(fontSize))
					.DisposeWith(disposables);

				//color
				this.OneWayBind(ViewModel, vm => vm.IsTextColorChanging, v => v._changeTextColorButton.Selected).DisposeWith(disposables);
				this.BindCommand(ViewModel, vm => vm.ChangeTextColor, v => v._changeTextColorButton).DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.IsTextColorChanging)
					.Select(selected => selected ? ViewStates.Visible : ViewStates.Gone)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(visibility => _textColorPicker.Visibility = visibility)
					.DisposeWith(disposables);
				
				this.WhenAnyValue(v => v.ViewModel.IsTextColorChanging)
					.Where(x => x)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(_ => _textColorPicker.SetColor(ViewModel.TextColor.ToIntColor()))
					.DisposeWith(disposables);
				
				IObservable<EventPattern<int>> textColorChangedObservable = Observable.FromEventPattern<EventHandler<int>, int>(handler => _textColorPicker.OnColorChanged += handler, handler => _textColorPicker.OnColorChanged -= handler);
				textColorChangedObservable.Select(color => color.EventArgs.StringFromColor())
					.Subscribe(color => ViewModel.UpdateTextColor.Execute(color))
					.DisposeWith(disposables);

				//preview
				this.WhenAnyValue(v => v.ViewModel.TopColor)
					.Select(colorString => colorString.ToColor())
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(color => _fontPreviewTop.SetBackgroundColor(color))
					.DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.BottomColor)
					.Select(colorString => colorString.ToColor())
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(color => _fontPreviewBottom.SetBackgroundColor(color))
					.DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.TextColor)
					.Select(colorString => colorString.ToColor())
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(color =>
					{
						_fontPreviewTop.SetTextColor(color);
						_fontPreviewBottom.SetTextColor(color);
					}).DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.FontSize)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(size =>
					{
						_fontPreviewTop.SetTextSize(ComplexUnitType.Sp, size);
						_fontPreviewBottom.SetTextSize(ComplexUnitType.Sp, size);
						
						int pixelSize = (int)TypedValue.ApplyDimension(ComplexUnitType.Sp, size, Resources.DisplayMetrics) * 2;
						_fontPreviewTop.LayoutParameters.Height = pixelSize;
						_fontPreviewBottom.LayoutParameters.Height = pixelSize;
					}).DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.SelectedFont)
					.Select(fontName => Typeface.Create(fontName, TypefaceStyle.Normal))
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(font =>
					{
						_fontPreviewTop.SetTypeface(font, TypefaceStyle.Normal);
						_fontPreviewBottom.Typeface = font;
					}).DisposeWith(disposables);

				#endregion

				#region Reinforcer

				this.OneWayBind(ViewModel, vm => vm.IsReinforcerColorChanging, v => v._changeReinforcerColorButton.Selected).DisposeWith(disposables);
				this.BindCommand(ViewModel, vm => vm.ChangeReinforcerColor, v => v._changeReinforcerColorButton).DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.IsReinforcerColorChanging)
					.Select(selected => selected ? ViewStates.Visible : ViewStates.Gone)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(visibility => _reinforcerColorPicker.Visibility = visibility)
					.DisposeWith(disposables);

				this.WhenAnyValue(v => v.ViewModel.IsReinforcerColorChanging)
					.Where(x => x)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(_ => _reinforcerColorPicker.SetColor(ViewModel.ReinforcerColor.ToIntColor()))
					.DisposeWith(disposables);
				
				this.WhenAnyValue(v => v.ViewModel.IsReinforcerEnabled)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(enabled => _isReinforcerEnabledCheckBox.Checked = enabled)
					.DisposeWith(disposables);

				IObservable<EventPattern<int>> reinforcerColorChangedObservable = Observable.FromEventPattern<EventHandler<int>, int>(handler => _reinforcerColorPicker.OnColorChanged += handler, handler => _reinforcerColorPicker.OnColorChanged -= handler);
				reinforcerColorChangedObservable.Select(color => color.EventArgs.StringFromColor())
					.Subscribe(color => ViewModel.UpdateReinforcerColor.Execute(color))
					.DisposeWith(disposables);

				IObservable<EventPattern<CompoundButton.CheckedChangeEventArgs>> isReinforcerCheckedObservable = Observable.FromEventPattern<EventHandler<CompoundButton.CheckedChangeEventArgs>, CompoundButton.CheckedChangeEventArgs>(handler => _isReinforcerEnabledCheckBox.CheckedChange += handler, handler => _isReinforcerEnabledCheckBox.CheckedChange -= handler);
				isReinforcerCheckedObservable.Select(enabled => enabled.EventArgs.IsChecked)
					.Subscribe(enabled => ViewModel.IsReinforcerEnabled = enabled)
					.DisposeWith(disposables);

				#endregion
			});
		}
	}
}