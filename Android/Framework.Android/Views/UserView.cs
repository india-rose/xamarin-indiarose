using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Framework.Converters;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Android.Graphics.Drawables;

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

        public ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
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


        protected UserView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public UserView(Context context)
            : base(context)
        {
        }

        public UserView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public UserView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
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

        public void Init(int availableHeight, int width)
        {
            TopView = new IndiagramBrowserView(Context);
            BotView = new SentenceAreaView(Context);

            AddView(TopView, width, (int)Math.Round(availableHeight * (SettingsService.SelectionAreaHeight / 100.0)));
            AddView(BotView, width, (int)Math.Round(availableHeight * (1 - (SettingsService.SelectionAreaHeight / 100.0))));

            BotView.SetY(TopView.LayoutParameters.Height);

            var colorconverter = new ColorStringToDrawableColorConverter();
            TopView.Background = (Drawable)colorconverter.Convert(SettingsService.TopBackgroundColor, null, null, null);
        }
    }
}