using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Views;
using Android.Widget;
using IndiaRose.Core;
using IndiaRose.Core.Admins.ViewModels;
using IndiaRose.Droid.Extensions;
using IndiaRose.Droid.Helpers;
using ReactiveUI;

namespace IndiaRose.Droid.Views.Settings
{
	public class SettingsPreviewFragment : BaseFragment<SettingsViewModel>
	{
		private View _topView;
		private View _bottomView;
		private ImageView _indiagramTopSample;
		private ImageView _indiagramBottomSample;
		private ImageView _nextButton;
		private ImageView _playButton;

		private View _reinforcerBackground;

		public SettingsPreviewFragment() : base(Resource.Layout.SettingsPreviewFragment)
		{
			
		}

		protected override void BindControls()
		{
			base.BindControls();

			_topView = RootView.FindViewById(Resource.Id.topBackground);
			_bottomView = RootView.FindViewById(Resource.Id.bottomBackground);

			_indiagramTopSample = RootView.FindViewById<ImageView>(Resource.Id.indiagramTopSample);
			_indiagramBottomSample = RootView.FindViewById<ImageView>(Resource.Id.indiagramBottomSample);
			_nextButton = RootView.FindViewById<ImageView>(Resource.Id.nextButton);
			_playButton = RootView.FindViewById<ImageView>(Resource.Id.playButton);

			_reinforcerBackground = RootView.FindViewById(Resource.Id.ReinforcerPreviewColor);

			this.WhenActivated(disposables =>
			{
				this.WhenAnyValue(x => x.ViewModel.BottomColor)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Select(x => x.ToColor())
					.Subscribe(color => _bottomView.SetBackgroundColor(color))
					.DisposeWith(disposables);

				this.WhenAnyValue(x => x.ViewModel.TopColor)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Select(x => x.ToColor())
					.Subscribe(color => _topView.SetBackgroundColor(color))
					.DisposeWith(disposables);

				this.WhenAnyValue(x => x.ViewModel.ReinforcerColor)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Select(x => x.ToColor())
					.Subscribe(color => _reinforcerBackground.SetBackgroundColor(color))
					.DisposeWith(disposables);

				this.WhenAnyValue(x => x.ViewModel.IsReinforcerEnabled)
					.Select(enabled => enabled ? ViewStates.Visible : ViewStates.Gone)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(visibility => _reinforcerBackground.Visibility = visibility)
					.DisposeWith(disposables);

				this.WhenAnyValue(x => x.ViewModel.IndiagramSize)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(indiagramSize =>
					{
						float availableSize = DimensionsHelper.DpToPixels(120);
						float deviceSize = ServiceLocator.DeviceInfoService.Height;

						int previewSize = (int)(indiagramSize * availableSize / deviceSize);
						int reinforcerSize = (int) (previewSize * 1.2);



						SetDimension(_indiagramTopSample, previewSize);
						SetDimension(_indiagramBottomSample, previewSize);
						SetDimension(_playButton, previewSize);
						SetDimension(_nextButton, previewSize);


						int initialMargin = (int)DimensionsHelper.DpToPixels(8);
						int margin = initialMargin - (int)(previewSize * 0.1);
						(_reinforcerBackground.LayoutParameters as RelativeLayout.LayoutParams)?.SetMargins(margin, margin, margin, margin);
						SetDimension(_reinforcerBackground, reinforcerSize);

					}).DisposeWith(disposables);

				this.WhenAnyValue(x => x.ViewModel.BottomSize)
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(bottomSize =>
					{
						float availableSize = DimensionsHelper.DpToPixels(120);
						float deviceSize = ServiceLocator.DeviceInfoService.Height;

						_bottomView.LayoutParameters.Height = (int) (bottomSize * availableSize / deviceSize + DimensionsHelper.DpToPixels(16));
						_bottomView.RequestLayout();
					}).DisposeWith(disposables);
			});
		}

		private void SetDimension(View view, int size)
		{
			view.LayoutParameters.Height = size;
			view.LayoutParameters.Width = size;
			view.RequestLayout();
		}

		protected override SettingsViewModel CreateViewModel()
		{
			return null; //on purpose, will be affected from parent
		}
	}
}