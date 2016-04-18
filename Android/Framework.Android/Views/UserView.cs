#pragma warning disable 618
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
#if __ANDROID_16__
using Android.OS;
#endif

namespace IndiaRose.Framework.Views
{
    /// <summary>
    /// Affiche la partie utilisateur
    /// Est composé de 2 parties : IndiagramBrowserView et SentenceAreaView
    /// </summary> 
    /// <see cref="IndiagramBrowserView"/>
    /// <see cref="SentenceAreaView"/>
	public class UserView : AbsoluteLayout
	{
		#region Services
		public ISettingsService SettingsService
		{
			get { return LazyResolver<ISettingsService>.Service; }
		}

		protected ITextToSpeechService TextToSpeechService
		{
			get { return LazyResolver<ITextToSpeechService>.Service; }
		}

		#endregion

		#region Private field

		private IndiagramBrowserView _topView;
		private SentenceAreaView _botView;
		private double _botViewYOffset;

		#endregion

		public event EventHandler TopCountChanged;
		public event EventHandler BotCanAddIndiagramsChanged;

		#region Properties

		#region Top Properties

		public Drawable TopBackground
		{
			get { return _topView.Background; }
			set
			{
#if __ANDROID_16__
				if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
				{
					_topView.Background = value;
					return;
			}
#endif
				_topView.SetBackgroundDrawable(value);
			}
		}

		public int TopCount
		{
			get { return _topView.Count; }
			set { _topView.Count = value; }
		}

		public int TopOffset
		{
			get { return _topView.Offset; }
			set { _topView.Offset = value; }
		}

		public List<Indiagram> TopIndiagrams
		{
			get { return _topView.Indiagrams; }
			set { _topView.Indiagrams = value; }
		}

		public uint TopTextColor
		{
			get { return _topView.TextColor; }
			set { _topView.TextColor = value; }
		}
		public ICommand TopIndiagramSelectedCommand { get; set; }

		public ICommand TopIndiagramDragStartCommand { get; set; }

		public ICommand TopNextCommand
		{
			get { return _topView.NextCommand; }
			set { _topView.NextCommand = value; }
		}

		public ViewStates TopNextButtonVisibility
		{
			get { return _topView.NextButton.Visibility; }
			set { _topView.NextButton.Visibility = value; }
		}

		public bool IsCorrectionModeEnabled { get; set; }

		#endregion

		#region Bot Properties
		public Drawable BotBackground
		{
			get { return _botView.Background; }
			set
			{
#if __ANDROID_16__
				if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
				{
					_botView.Background = value;
					return;
				}
#endif
				_botView.SetBackgroundDrawable(value);
			}
		}

		public ObservableCollection<IndiagramUIModel> BotIndiagrams
		{
			get { return _botView.Indiagrams; }
			set { _botView.Indiagrams = value; }
		}

		public bool BotCanAddIndiagrams
		{
			get { return _botView.CanAddIndiagrams; }
			set { _botView.CanAddIndiagrams = value; }
		}

		public ICommand BotReadCommand
		{
			get { return _botView.ReadCommand; }
			set { _botView.ReadCommand = value; }
		}

		public ICommand BotIndiagramSelectedCommand
		{
			get { return _botView.IndiagramSelectedCommand; }
			set { _botView.IndiagramSelectedCommand = value; }
		}

		public ICommand BotCorrectionCommand
		{
			get { return _botView.CorrectionCommand; }
			set { _botView.CorrectionCommand = value; }
		}
		#endregion

		#endregion

		#region Constructor
		protected UserView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
			Initialize();
		}

		public UserView(Context context)
			: base(context)
		{
			Initialize();
		}

		public UserView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			Initialize();
		}

		public UserView(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{
			Initialize();
		}
		private void Initialize()
		{
			_topView = new IndiagramBrowserView(Context)
			{
				IndiagramSelected = new DelegateCommand<Indiagram>(OnTopIndiagramSelected),
				IndiagramViewSelectedCommand = new DelegateCommand<IndiagramView>(OnTopIndiagramViewTouched),
			};
			_botView = new SentenceAreaView(Context);

			_topView.CountChanged += (sender, args) => this.RaiseEvent(TopCountChanged);
			_botView.CanAddIndiagramsChanged += (s, e) => this.RaiseEvent(BotCanAddIndiagramsChanged);

		}

		#endregion

        /// <summary>
        /// Initialise la vue
        /// </summary>
        /// <param name="availableHeight">Hauteur disponible à l'écran</param>
        /// <param name="width">Largeur de l'écran</param>
		public void Init(int availableHeight, int width)
		{
			int topHeight = (int)Math.Round(availableHeight * (SettingsService.SelectionAreaHeight / 100.0));
			int bottomHeight = availableHeight - topHeight;

			AddView(_topView, new LayoutParams(width, topHeight, 0, 0));
			AddView(_botView, new LayoutParams(width, bottomHeight, 0, topHeight));

			_botViewYOffset = topHeight;
		}

        /// <summary>
        /// Callback lorsqu'un Indiagram est sélectionné dans la partie collection (partie haute)
        /// Lorsqu'il n'y a pas le drag & drop
        /// </summary>
        /// <param name="indiagram">Indiagram sélectionné</param>
		private void OnTopIndiagramSelected(Indiagram indiagram)
		{
			if (!SettingsService.IsDragAndDropEnabled || indiagram.IsCategory)
			{
				var command = TopIndiagramSelectedCommand;
				if (command != null && command.CanExecute(indiagram))
				{
					command.Execute(indiagram);
				}
			}
		}

		private IndiagramView _currentView;

        /// <summary>
        /// Callback lorsqu'un Indiagram est sélectionné dans la partie collection (partie haute)
        /// Lorsqu'il y a le drag & drop
        /// </summary>
        /// <param name="view">Indiagram sélectionné</param>
		private void OnTopIndiagramViewTouched(IndiagramView view)
		{
			if (SettingsService.IsDragAndDropEnabled && !view.Indiagram.IsCategory && !TextToSpeechService.IsReading)
			{
				Indiagram indiagram = view.Indiagram;
				// command should only be executed when the indiagram is "dropped" in sentence view
				if (!SettingsService.IsMultipleIndiagramSelectionEnabled || IsCorrectionModeEnabled)
				{
					_topView.HideIndiagram(indiagram);
				}

				// get existing view
				view.Touch += OnIndiagramTouched;
				_currentView = view;
				_topView.SwitchViewForDragAndDrop(view);


				float left = _currentView.LastTouchArgs.Event.RawX;
				float top = _currentView.LastTouchArgs.Event.RawY;

				// attach to new layout
				AddView(view, new LayoutParams(IndiagramView.DefaultWidth, _currentView.RealHeight, (int)(left - IndiagramView.DefaultWidth / 2f), (int)(top - _currentView.RealHeight /2f)));
				_topView.RefreshView();

				if (TopIndiagramDragStartCommand != null && TopIndiagramDragStartCommand.CanExecute(indiagram))
				{
					TopIndiagramDragStartCommand.Execute(indiagram);
				}
			}
		}

        /// <summary>
        /// Callback lorsqu'un Indiagram est cliqué
        /// </summary>
		private void OnIndiagramTouched(object sender, TouchEventArgs touchEventArgs)
		{
			IndiagramView view = sender as IndiagramView;

			if (view == null || view != _currentView)
			{
				return;
			}

			if (touchEventArgs.Event.ActionMasked == MotionEventActions.Move)
			{
				float left = touchEventArgs.Event.RawX;
				float top = touchEventArgs.Event.RawY;
				_currentView.LayoutParameters = new LayoutParams(IndiagramView.DefaultWidth, _currentView.RealHeight, (int)(left - IndiagramView.DefaultWidth / 2f), (int)(top - _currentView.RealHeight /2f));
			}
			else if (touchEventArgs.Event.ActionMasked == MotionEventActions.Up)
			{
				bool selected = touchEventArgs.Event.RawY >= _botViewYOffset;
				
				//in any case, remove from the current view
				_currentView.Touch -= OnIndiagramTouched;
				RemoveView(_currentView);
				_currentView = null;

				if(selected)
				{
					var command = TopIndiagramSelectedCommand;
					if (command != null && command.CanExecute(view.Indiagram))
					{
						command.Execute(view.Indiagram);
					}
				}

				if (!SettingsService.IsMultipleIndiagramSelectionEnabled)
				{
					_topView.ShowAllIndiagrams();
					_topView.RefreshView();
				}
			}
		}
	}
}