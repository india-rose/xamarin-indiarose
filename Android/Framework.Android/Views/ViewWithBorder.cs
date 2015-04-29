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

        /**
        * Current width of the view.
        */
        private int _width;
        /**
         * Current height of the view.
         */
        private int _height;

        public int BorderSize { get; set; }

        public Color BorderColor
        {
            get { return _borderColor;}
            set
            {
                _borderColor = new Color(value);
            }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = new Color(value);
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

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            SetMeasuredDimension(_width, _height);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawRect(0,0,_width,_height, new Paint
            {
                Color=BorderColor
            });
            canvas.DrawRect(1, 1, _width - 1, _height - 1, new Paint
            {
                Color = BackgroundColor
            });
            
        }
    }
}