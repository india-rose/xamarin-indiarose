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
		public ReactiveCommand<Unit, bool> Save { get; private set; }
		public ReactiveCommand<Unit, Unit> ChangeTopColor { get; private set; }
		public ReactiveCommand<Unit, Unit> ChangeBottomColor { get; private set; }

		private readonly ObservableAsPropertyHelper<int> _bottomSize;

		private string _topColor;
		private string _bottomColor;
		private int _indiagramSize;
		private int _fontSize;

		public string TopColor
		{
			get { return _topColor; }
			set { this.RaiseAndSetIfChanged(ref _topColor, value); }
		}

		public int IndiagramSize
		{
			get { return _indiagramSize; }
			set { this.RaiseAndSetIfChanged(ref _indiagramSize, value); }
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

		public int BottomSize => _bottomSize.Value;

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
			Save = ReactiveCommand.CreateFromTask(async _ =>
			{
				await Task.Delay(1000);
				return new Random(DateTime.Now.Millisecond).Next(0, 10) < 5;
			});

			ChangeTopColor = ReactiveCommand.Create(() => { IsTopColorChanging = !IsTopColorChanging; });
			ChangeBottomColor = ReactiveCommand.Create(() => { IsBottomColorChanging = !IsBottomColorChanging; });

			//compute correct size
			_bottomSize = this.WhenAny(x => x.IndiagramSize, indiagram => (int)(indiagram.Value * 1.2))
								.ToProperty(this, x => x.FontSize);

			this.WhenActivated(disposables =>
			{
				Observable.FromAsync(ct => ServiceLocator.SettingsService.Load(ct))
					.Subscribe(settings =>
					{
						TopColor = settings.TopBackgroundColor;
						BottomColor = settings.BottomBackgroundColor;
						IndiagramSize = settings.IndiagramDisplaySize;
						FontSize = settings.FontSize;
					}).DisposeWith(disposables);

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
	}
}
