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
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
#pragma warning disable 618
    public class UserView : AbsoluteLayout
#pragma warning restore 618
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
		public ICommand TopIndiagramSelectedCommand
		{
			get { return _topView.IndiagramSelected; }
			set { _topView.IndiagramSelected = value; }
		}

		public ICommand TopNextCommand
		{
			get { return _topView.NextCommand; }
			set { _topView.NextCommand = value; }
		}

		public ViewStates TopNextButtonVisibility {
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
            _topView = new IndiagramBrowserView(Context);
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
    }
}