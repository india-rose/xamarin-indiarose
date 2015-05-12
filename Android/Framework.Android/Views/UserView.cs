using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Android.Graphics.Drawables;
using IndiaRose.Data.Model;

namespace IndiaRose.Framework.Views
{
#pragma warning disable 618
    public class UserView : AbsoluteLayout
#pragma warning restore 618
    {
        #region Services
        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }

        public ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }
        #endregion
        #region Private field

        private IndiagramBrowserView _topView;
        private SentenceAreaView _botView;

        #endregion

        public event EventHandler CountChanged;
        #region Properties
        public IndiagramBrowserView TopView
        {
            get { return _topView; }
            set { SetProperty(ref _topView, value); }
        }

        public SentenceAreaView BotView
        {
            get { return _botView; }
            set { SetProperty(ref _botView, value); }
        }

        public Drawable TopBackground
        {
            get { return TopView.Background; }
            set { TopView.Background = value; }
        }

        public Drawable BotBackground
        {
            get { return BotView.Background; }
            set { BotView.Background = value; }
        }

        public int TopCount
        {
            get { return TopView.Count; }
            set
            {
                TopView.Count = value; 
                EventHandler handler = CountChanged;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        public int TopOffset
        {
            get { return TopView.Offset; }
            set { TopView.Offset = value; }
        }

        public List<Indiagram> TopIndiagrams
        {
            get { return TopView.Indiagrams; }
            set
            {
                TopView.Indiagrams = value;
                Post(Invalidate);
            }
        }

        public uint TopTextColor
        {
            get { return TopView.TextColor; }
            set { TopView.TextColor = value; }
        }
        public ICommand TopIndiagramSelected
        {
            get { return TopView.IndiagramSelected; }
            set { TopView.IndiagramSelected = value; }
        }

        public ICommand TopNextCommand
        {
            get { return TopView.NextCommand; }
            set { TopView.NextCommand = value; }
        }
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
            TopView = new IndiagramBrowserView(Context);
            BotView = new SentenceAreaView(Context);
        }
        #endregion

        public void Init(int availableHeight, int width)
        {

            AddView(TopView, width, (int)Math.Round(availableHeight * (SettingsService.SelectionAreaHeight / 100.0)));
            AddView(BotView, width, (int)Math.Round(availableHeight * (1 - (SettingsService.SelectionAreaHeight / 100.0))));

            BotView.SetY(TopView.LayoutParameters.Height);
        }

        #region Private tools methods

        private bool SetProperty<T>(ref T storage, T value)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            return true;
        }

        private bool SetProperty<T>(ref T storage, T value, Func<EventHandler> eventGetter)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;

            EventHandler handler = eventGetter();
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            return true;
        }

        #endregion
    }
}