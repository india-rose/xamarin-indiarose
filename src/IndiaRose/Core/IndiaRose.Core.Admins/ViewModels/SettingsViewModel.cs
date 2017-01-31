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
		public ReactiveCommand<string, Unit> UpdateBackgroundColor { get; }
		public ReactiveCommand<int, Unit> UpdateIndiagramSize { get; }
		public ReactiveCommand<int, Unit> SelectFont { get; }
		public ReactiveCommand<int, Unit> UpdateFontSize { get; }

		private readonly ObservableAsPropertyHelper<int> _bottomSize;
		private readonly ObservableAsPropertyHelper<int> _indiagramSize;

		private readonly double _indiagramMaxSize;

		private string _topColor;
		private string _bottomColor;
		private int _indiagramSizePercentage;
		private int _fontSize;
		private List<string> _fontNames;
		private string _selectedFont;

		public string TopColor
		{
			get { return _topColor; }
			set { this.RaiseAndSetIfChanged(ref _topColor, value); }
		}

		public int IndiagramSizePercentage
		{
			get { return _indiagramSizePercentage; }
			set { this.RaiseAndSetIfChanged(ref _indiagramSizePercentage, value); }
		}

		public string BottomColor
		{
			get { return _bottomColor; }
			set { this.RaiseAndSetIfChanged(ref _bottomColor, value); }
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

		public int BottomSize => _bottomSize.Value;

		public int IndiagramSize => _indiagramSize.Value;

		private bool _isTopColorChanging;
		private bool _isBottomColorChanging;
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
			UpdateBackgroundColor = ReactiveCommand.Create<string>(UpdateBackgroundColorAction);
			UpdateIndiagramSize = ReactiveCommand.Create<int>(UpdateIndiagramSizeAction);
			SelectFont = ReactiveCommand.Create<int>(SelectFontAction);
			UpdateFontSize = ReactiveCommand.Create<int>(UpdateFontSizeAction);

			//compute correct size
			_indiagramSize = this.WhenAnyValue(x => x.IndiagramSizePercentage)
								.Select(percent => (int) (percent / 100.0 * _indiagramMaxSize))
								.ToProperty(this, x => x.IndiagramSize);
			_bottomSize = this.WhenAnyValue(x => x.IndiagramSize)
								.Select(indiagramSize => (int)(indiagramSize * 1.2))
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
