using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Framework.Converters;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Framework.Views
{
	public class SentenceAreaView : RelativeLayout
	{
		public event EventHandler CanAddIndiagramsChanged;

		private bool _canAddIndiagrams = true;
		private int _viewId = 0xFFFF2a;
		private int _maxNumberOfIndiagrams;
		private IndiagramView _playButton;
		private ObservableCollection<IndiagramUIModel> _indiagrams;
		private readonly List<IndiagramView> _indiagramViews = new List<IndiagramView>();

		public ICommand IndiagramSelectedCommand { get; set; }

		public ICommand ReadCommand { get; set; }

		public ObservableCollection<IndiagramUIModel> Indiagrams
		{
			get { return _indiagrams; }
			set
			{
				if (!Equals(_indiagrams, value))
				{
					if (_indiagrams != null)
					{
						_indiagrams.CollectionChanged -= IndiagramsOnCollectionChanged;
					}
					_indiagrams = value;
					if (_indiagrams != null)
					{
						_indiagrams.CollectionChanged += IndiagramsOnCollectionChanged;
					}
				}
			}
		}

		public bool CanAddIndiagrams
		{
			get { return _canAddIndiagrams; }
			set
			{
				if (_canAddIndiagrams != value)
				{
					_canAddIndiagrams = value;
					this.RaiseEvent(CanAddIndiagramsChanged);
				}
			}
		}

		#region Constructors

		public SentenceAreaView(Context context)
			: base(context)
		{
			Initialize();
		}

		public SentenceAreaView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			Initialize();
		}

		public SentenceAreaView(Context context, IAttributeSet attrs, int defStyleAttr)
			: base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		#endregion

		private void Initialize()
		{
			Id = _viewId;
			_maxNumberOfIndiagrams = LazyResolver<IScreenService>.Service.Width / IndiagramView.DefaultWidth - 1;
			ISettingsService settings = LazyResolver<ISettingsService>.Service;
			ColorStringToIntConverter colorConverter = new ColorStringToIntConverter();

			// Init views
			for (int i = 0; i < _maxNumberOfIndiagrams; ++i)
			{
				IndiagramView view = new IndiagramView(Context)
				{
					TextColor = (uint) colorConverter.Convert(settings.TextColor, null, null, null),
					Id = _viewId++,
					DefaultColor = 0,
				};
				view.Touch += OnIndiagramTouched;

				var layoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
				layoutParams.AddRule(LayoutRules.CenterVertical);
				if (i == 0)
				{
					layoutParams.AddRule(LayoutRules.AlignParentLeft);
				}
				else
				{
					layoutParams.AddRule(LayoutRules.RightOf, _viewId - 2);
				}

				AddView(view, layoutParams);
				_indiagramViews.Add(view);
			}


			// Init play button
			_playButton = new IndiagramView(Context)
			{
				TextColor = 0,
				Id = _viewId++,
				Indiagram = new Indiagram()
				{
					Text = "play",
					ImagePath = LazyResolver<IStorageService>.Service.ImagePlayButtonPath
				}
			};

			_playButton.Touch += (sender, args) =>
			{
				if (args.Event.ActionMasked == MotionEventActions.Down)
				{
					if (ReadCommand != null && ReadCommand.CanExecute(null))
					{
						ReadCommand.Execute(null);
					}
				}
			};

			var lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			lp.AddRule(LayoutRules.AlignParentRight);
			lp.AddRule(LayoutRules.CenterVertical);

			AddView(_playButton, lp);
		}

		private void IndiagramsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (IndiagramUIModel model in e.OldItems)
				{
					model.ReinforcerStateChanged -= IndiagramReinforcerChanged;
				}
			}

			if (e.NewItems != null)
			{
				foreach (IndiagramUIModel model in e.NewItems)
				{
					model.ReinforcerStateChanged += IndiagramReinforcerChanged;
				}
			}

			//refresh everything
			int i = 0;
			for (; i < _indiagrams.Count; ++i)
			{
				_indiagramViews[i].Indiagram = _indiagrams[i].Model;
			}
			for (; i < _indiagramViews.Count; ++i)
			{
				_indiagramViews[i].Indiagram = null;
			}
			CanAddIndiagrams = (_indiagrams.Count < _indiagramViews.Count);
		}

		private void IndiagramReinforcerChanged(object sender, EventArgs eventArgs)
		{
			IndiagramUIModel uiModel = sender as IndiagramUIModel;

			if (uiModel != null)
			{
				// find the view associated with this model
				IndiagramView view = _indiagramViews.FirstOrDefault(x => x.Indiagram != null && Indiagram.AreSameIndiagram(x.Indiagram, uiModel.Model));

				if (view == null)
				{
					LazyResolver<ILoggerService>.Service.Log("SentenceAreaView : got a reinforcerChangedEvent from a non registered indiagram !", MessageSeverity.Critical);
					return;
				}

				if (uiModel.IsReinforcerEnabled)
				{
					view.BackgroundColor = (uint)(new ColorStringToIntConverter().Convert(
						LazyResolver<ISettingsService>.Service.ReinforcerColor, null, null, null));	
				}
				else
				{
					// set it back to transparent
					view.BackgroundColor = 0x0;
				}
			}
		}

		private void OnIndiagramTouched(object sender, TouchEventArgs e)
		{
			if (e.Event.ActionMasked == MotionEventActions.Down)
			{
				IndiagramView view = sender as IndiagramView;
				if (view != null && view.Indiagram != null && IndiagramSelectedCommand != null && IndiagramSelectedCommand.CanExecute(view.Indiagram))
				{
					IndiagramSelectedCommand.Execute(view.Indiagram);
				}
			}
		}
	}
}