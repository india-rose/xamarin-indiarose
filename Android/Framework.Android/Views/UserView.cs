using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
#pragma warning disable 618
    public class UserView : AbsoluteLayout
#pragma warning restore 618
    {
        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }
        public IndiagramBrowserView TopView
        {
            get { return _topView; } 
            set { SetProperty(ref _topView, value); }
        }
        private IndiagramBrowserView _topView;

        public SentenceAreaView BotView
        {
            get { return _botView; }
            set { SetProperty(ref _botView, value); }
        }
        private SentenceAreaView _botView;


        protected UserView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public UserView(Context context) : base(context)
        {
            Initialize();
        }

        public UserView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public UserView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            if (CollectionStorageService.IsInitialized) CollectionStorageService_Initialized(this, null);
            else CollectionStorageService.Initialized += CollectionStorageService_Initialized;
        }

        private void CollectionStorageService_Initialized(object sender, EventArgs e)
        {
            TopView = new IndiagramBrowserView(Context);
            BotView = new SentenceAreaView(Context);
            AddView(TopView);
            AddView(BotView);
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