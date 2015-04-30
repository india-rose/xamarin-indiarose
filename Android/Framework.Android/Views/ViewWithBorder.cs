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
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class ViewWithBorder : View
    {
        protected static ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }

        private Color _borderColor;
        private Color _backgroundColor;


        public int BorderSize { get; set; }

        public Color BorderColor
        {
            get { return _borderColor;}
            set
            {
                _borderColor = value;
            }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                Post(Invalidate);
            }
        }

        protected ViewWithBorder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ViewWithBorder(Context context) : base(context)
        {
        }

        public ViewWithBorder(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ViewWithBorder(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawRect(0,0,Width,Height, new Paint
            {
                Color=Color.Black
            });
            canvas.DrawRect(1, 1, Width - 1, Height - 1, new Paint
            {
                Color = BackgroundColor
            });
            
        }
    }
}