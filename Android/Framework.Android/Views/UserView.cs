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

namespace IndiaRose.Framework.Views
{
    public class UserView : AbsoluteLayout
    {
        #region Services
        public ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }
        #endregion

        #region Private field

        private IndiagramBrowserView _topView;
        private SentenceAreaView _botView;

        #endregion

        public event EventHandler TopCountChanged;
		public event EventHandler BotCanAddIndiagramsChanged;

        #region Properties

		#region Top Properties
		
		public Drawable TopBackground
		{
			get { return _topView.Background; }
			set { _topView.Background = value; }
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

		public ICommand TopNextCommand
		{
			get { return _topView.NextCommand; }
			set { _topView.NextCommand = value; }
		}

		public ViewStates TopNextButtonVisibility 
		{
			get{ return _topView.NextButton.Visibility; }
			set{ _topView.NextButton.Visibility = value; }
		}

		#endregion

        #region Bot Properties
        public Drawable BotBackground
        {
			get { return _botView.Background; }
			set { _botView.Background = value; }
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

	    public void Init(int availableHeight, int width)
        {
	        int topHeight = (int) Math.Round(availableHeight*(SettingsService.SelectionAreaHeight/100.0));
	        int bottomHeight = availableHeight - topHeight;

			AddView(_topView, width, topHeight);
			AddView(_botView, width, bottomHeight);

			_botView.SetY(_topView.LayoutParameters.Height);
        }

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
		private void OnTopIndiagramViewTouched(IndiagramView view)
		{
			if (SettingsService.IsDragAndDropEnabled && !view.Indiagram.IsCategory)
			{
				Indiagram indiagram = view.Indiagram;
				// command should only be executed when the indiagram is "dropped" in sentence view
				if (!SettingsService.IsMultipleIndiagramSelectionEnabled)
				{
					_topView.HideIndiagram(indiagram);
				}

				// get existing view
				view.Touch += OnIndiagramTouched;
				_currentView = view;
				float left = view.GetX();
				float top = view.GetY();
				_topView.SwitchViewForDragAndDrop(view);

				// attach to new layout
				AddView(view);
				view.SetX(left + view.Width / 2f);
				view.SetY(top + view.Height / 2f);
				_topView.RefreshView();
			}
		}

	    private void OnIndiagramTouched(object sender, TouchEventArgs touchEventArgs)
	    {
		    IndiagramView view = sender as IndiagramView;

		    if (view == null || view != _currentView)
		    {
			    return;
		    }

		    if (touchEventArgs.Event.ActionMasked == MotionEventActions.Move)
		    {
			    view.SetX(touchEventArgs.Event.RawX + view.Width / 2f);
				view.SetY(touchEventArgs.Event.RawY + view.Height / 2f);
		    }
			else if (touchEventArgs.Event.ActionMasked == MotionEventActions.Up)
			{
				if (view.GetY() + view.Height / 2.0 >= _botView.GetY())
				{
					var command = TopIndiagramSelectedCommand;
					if (command != null && command.CanExecute(view.Indiagram))
					{
						command.Execute(view.Indiagram);
					}
				}

				_currentView = null;
				//in any case, remove from the current view
				view.Touch -= OnIndiagramTouched;
				RemoveView(view);

				if (!SettingsService.IsMultipleIndiagramSelectionEnabled)
				{
					_topView.ShowAllIndiagrams();
					_topView.RefreshView();
				}
			}
	    }
    }
}