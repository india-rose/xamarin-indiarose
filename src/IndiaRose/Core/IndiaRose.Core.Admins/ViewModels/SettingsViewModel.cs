using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace IndiaRose.Core.Admins.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		public ReactiveCommand<Unit, bool> Save { get; }
		public ReactiveCommand<Unit, Unit> ChangeTopBackgroundColor { get; }
		public ReactiveCommand<Unit, Unit> ChangeBottomBackgroundColor { get; }
		public ReactiveCommand<Unit, Unit> ChangeTextColor { get; }
		public ReactiveCommand<Unit, Unit> ChangeReinforcerColor { get; }
		public ReactiveCommand<string, Unit> UpdateBackgroundColor { get; }
		public ReactiveCommand<string, Unit> UpdateTextColor { get; }
		public ReactiveCommand<string, Unit> UpdateReinforcerColor { get; }
		public ReactiveCommand<int, Unit> UpdateIndiagramSize { get; }
		public ReactiveCommand<int, Unit> SelectFont { get; }
		public ReactiveCommand<int, Unit> UpdateFontSize { get; }

		private readonly ObservableAsPropertyHelper<int> _bottomSize;
		private readonly ObservableAsPropertyHelper<int> _indiagramSize;

		private readonly double _indiagramMaxSize;

		private string _topColor;
		private string _bottomColor;
		private string _textColor;
		private string _reinforcerColor;

		private int _indiagramSizePercentage;
		private int _fontSize;
		private List<string> _fontNames;
		private string _selectedFont;
		private bool _isReinforcerEnabled;

		public string TopColor
		{
			get { return _topColor; }
			set { this.RaiseAndSetIfChanged(ref _topColor, value); }
		}

		public string BottomColor
		{
			get { return _bottomColor; }
			set { this.RaiseAndSetIfChanged(ref _bottomColor, value); }
		}

		public string TextColor
		{
			get { return _textColor; }
			set { this.RaiseAndSetIfChanged(ref _textColor, value); }
		}

		public string ReinforcerColor
		{
			get { return _reinforcerColor; }
			set { this.RaiseAndSetIfChanged(ref _reinforcerColor, value); }
		}

		public int IndiagramSizePercentage
		{
			get { return _indiagramSizePercentage; }
			set { this.RaiseAndSetIfChanged(ref _indiagramSizePercentage, value); }
		}

		public int FontSize
		{
			get { return _fontSize; }
			set { this.RaiseAndSetIfChanged(ref _fontSize, value); }
		}

		public List<string> FontNames
		{
			get { return _fontNames; }
			set { this.RaiseAndSetIfChanged(ref _fontNames, value); }
		}

		public string SelectedFont
		{
			get { return _selectedFont; }
			set { this.RaiseAndSetIfChanged(ref _selectedFont, value); }
		}

		public bool IsReinforcerEnabled
		{
			get { return _isReinforcerEnabled; }
			set { this.RaiseAndSetIfChanged(ref _isReinforcerEnabled, value); }
		}

		public int BottomSize => _bottomSize.Value;

		public int IndiagramSize => _indiagramSize.Value;

		private bool _isTopColorChanging;
		private bool _isBottomColorChanging;
		private bool _isTextColorChanging;
		private bool _isReinforcerColorChanging;

		public bool IsBottomColorChanging
		{
			get { return _isBottomColorChanging; }
			set { this.RaiseAndSetIfChanged(ref _isBottomColorChanging, value); }
		}

		public bool IsTopColorChanging
		{
			get { return _isTopColorChanging; }
			set { this.RaiseAndSetIfChanged(ref _isTopColorChanging, value); }
		}

		public bool IsTextColorChanging
		{
			get { return _isTextColorChanging; }
			set { this.RaiseAndSetIfChanged(ref _isTextColorChanging, value); }
		}

		public bool IsReinforcerColorChanging
		{
			get { return _isReinforcerColorChanging; }
			set { this.RaiseAndSetIfChanged(ref _isReinforcerColorChanging, value); }
		}

		public SettingsViewModel()
		{
			_indiagramMaxSize = Math.Min(ServiceLocator.DeviceInfoService.Height, ServiceLocator.DeviceInfoService.Width) / 2.0 * 0.9;
			Save = ReactiveCommand.CreateFromTask(async _ =>
			{
				await Task.Delay(1000);
				return new Random(DateTime.Now.Millisecond).Next(0, 10) < 5;
			});

			ChangeTopBackgroundColor = ReactiveCommand.Create(() => { IsTopColorChanging = !IsTopColorChanging; });
			ChangeBottomBackgroundColor = ReactiveCommand.Create(() => { IsBottomColorChanging = !IsBottomColorChanging; });
			ChangeTextColor = ReactiveCommand.Create(() => { IsTextColorChanging = !IsTextColorChanging; });
			ChangeReinforcerColor = ReactiveCommand.Create(() => { IsReinforcerColorChanging = !IsReinforcerColorChanging; });
			UpdateBackgroundColor = ReactiveCommand.Create<string>(UpdateBackgroundColorAction);
			UpdateTextColor = ReactiveCommand.Create<string>(UpdateTextColorAction);
			UpdateReinforcerColor = ReactiveCommand.Create<string>(UpdateReinforcerAction);
			UpdateIndiagramSize = ReactiveCommand.Create<int>(UpdateIndiagramSizeAction);
			SelectFont = ReactiveCommand.Create<int>(SelectFontAction);
			UpdateFontSize = ReactiveCommand.Create<int>(UpdateFontSizeAction);

			//compute correct size
			_indiagramSize = this.WhenAnyValue(x => x.IndiagramSizePercentage)
								.Select(percent => (int) (percent / 100.0 * _indiagramMaxSize))
								.ToProperty(this, x => x.IndiagramSize);
			_bottomSize = this.WhenAnyValue(x => x.IndiagramSize)
								.Select(indiagramSize => (int)(indiagramSize))
								.ToProperty(this, x => x.BottomSize);

			this.WhenActivated(disposables =>
			{
				Observable.FromAsync(ct => ServiceLocator.SettingsService.Load(ct))
					.Subscribe(settings =>
					{
						TopColor = settings.TopBackgroundColor;
						BottomColor = settings.BottomBackgroundColor;
						IndiagramSizePercentage = settings.IndiagramSizePercentage;
						FontSize = settings.FontSize;
						SelectedFont = settings.FontName;
						TextColor = settings.TextColor;
						IsReinforcerEnabled = settings.IsReinforcerEnabled;
						ReinforcerColor = settings.ReinforcerColor;
					}).DisposeWith(disposables);

				Observable.FromAsync(ct => ServiceLocator.FontService.GetFontDisplayNames())
					.Subscribe(fontNames => FontNames = fontNames)
					.DisposeWith(disposables);

				this.WhenAnyValue(vm => vm.IsBottomColorChanging)
					.Where(x => x)
					.Subscribe(_ => IsTopColorChanging = false)
					.DisposeWith(disposables);

				this.WhenAnyValue(vm => vm.IsTopColorChanging)
					.Where(x => x)
					.Subscribe(_ => IsBottomColorChanging = false)
					.DisposeWith(disposables);
			});
		}

		private void UpdateTextColorAction(string color)
		{
			TextColor = color;
		}

		private void UpdateReinforcerAction(string color)
		{
			ReinforcerColor = color;
		}

		private void UpdateBackgroundColorAction(string color)
		{
			if (IsTopColorChanging)
			{
				TopColor = color;
			}
			else if (IsBottomColorChanging)
			{
				BottomColor = color;
			}
		}

		private void UpdateIndiagramSizeAction(int sizePercentage)
		{
			IndiagramSizePercentage = sizePercentage;
		}

		private void SelectFontAction(int fontIndex)
		{
			if (fontIndex >= 0 && fontIndex < FontNames.Count)
			{
				SelectedFont = FontNames[fontIndex];
			}
		}

		private void UpdateFontSizeAction(int fontSize)
		{
			FontSize = fontSize;
		}
	}
}
