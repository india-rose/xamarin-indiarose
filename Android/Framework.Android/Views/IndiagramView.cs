using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using IndiaRose.Data.Model;
using IndiaRose.Framework.Helper;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class IndiagramView : View
    {
        #region Fields

        /**
         * Paint object to draw the image.
         */
        private readonly Paint _picturePainter = new Paint();
        /**
         * Paint object to write the text.
         */
        private readonly Paint _textPainter = new Paint();
        /**
         * Paint object to draw background.
         */
        private readonly Paint _backgroundPainter = new Paint();
        /**
         * Rect of the view to draw background.
         */
        private Rect _backgroundRect;
        /**
         * Indiagram relative to this view.
         */
        private Indiagram _indiagram;
        /**
         * Current width of the view.
         */
        private int _width;
        /**
         * Current height of the view.
         */
        private int _height;
        /**
         * width of the image.
         */
        private int _pictureWidth;
        /**
         * height of the image.
         */
        private int _pictureHeight;
        /**
         * Left margin from left of the view to the image left edge.
         */
        private int _marginLeft;
        /**
         * Top margin from top of the view to the image top edge.
         */
        private int _marginTop;
        /**
         * Color of the text.
         */
		private uint _textColor;
        /**
         * Boolean to know if the text fill on one line or more.
         */
        private bool _isOneLineText = true;

		private uint _backgroundColor;
	    private uint _defaultColor;

	    #endregion

        #region Services

        protected static ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }

        #endregion

        #region Properties

        public Indiagram Indiagram
        {
            get { return _indiagram; }
            set
            {
                if (!Equals(_indiagram, value))
                {
                    _indiagram = value;
                    RefreshDimension();
                    Post(Invalidate);
                }
            }
        }

        public uint TextColor
        {
            get { return _textColor; }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    _textPainter.Color = new Color((int)_textColor);
                    Post(Invalidate);
                }
            }
        }

		public uint BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    _backgroundPainter.Color = new Color((int)value);
                    Post(Invalidate);
                }
            }
        }

	    public uint DefaultColor
	    {
			get { return _defaultColor; }
		    set
		    {
			    if (_defaultColor != value)
			    {
				    _defaultColor = value;
				    _picturePainter.Color = new Color((int) _defaultColor);
				    Post(Invalidate);
			    }
		    }
	    }

        public int RealHeight
        {
            get { return _height; }
        }

	    public TouchEventArgs LastTouchArgs { get; set; }

        #endregion

        #region Constructors

        protected IndiagramView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Initialize();
        }

        public IndiagramView(Context context)
            : base(context)
        {
            Initialize();
        }

        public IndiagramView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public IndiagramView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        #endregion

        private void Initialize()
        {
            _picturePainter.Color = Color.Red;
            _textPainter.SetTypeface(FontHelper.LoadFont(SettingsService.FontName));
            _textPainter.Color = new Color((int)_textColor);
            _textPainter.TextSize = SettingsService.FontSize;
            _textPainter.TextAlign = Paint.Align.Center;
            _backgroundPainter.Color = Color.Transparent;

            _pictureWidth = _pictureHeight = SettingsService.IndiagramDisplaySize;
	        _backgroundRect = _backgroundRect ?? new Rect(0, 0, 0, 0);
        }

        protected void RefreshDimension()
        {
	        if (_indiagram == null)
	        {
		        return;
	        }
            float textWidth = _textPainter.MeasureText(_indiagram.Text);
            int textHeight = SettingsService.FontSize;

            if (string.IsNullOrEmpty(_indiagram.Text))
            {
                textHeight = 0;
            }

            _marginLeft = _pictureWidth / 10;
            _marginTop = _pictureHeight / 10;

            _width = _marginLeft * 2 + _pictureWidth;
            //height take account the fact that some words will needs two or more lines to be displayed.
            _height = _marginTop * 2 + _pictureHeight + (textHeight * (int)(textWidth / (_pictureWidth + 1) + 1));

            SetMinimumWidth(_width);
            SetMinimumHeight(_height);
            Measure(_width, _height);

            _isOneLineText = (textWidth <= _pictureWidth);
            _backgroundRect = new Rect(0, 0, _width, _height);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            SetMeasuredDimension(_width, _height);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

	        if (_indiagram == null)
	        {
		        return;
	        }

            canvas.DrawRect(_backgroundRect, _backgroundPainter);


			if (!_indiagram.IsEnabled) {
				_picturePainter.Alpha = 128;
			} else {
				_picturePainter.Alpha = 255;
			}
            //draw picture or a red rectangle if error with picture
            try
            {
                Bitmap image = ImageHelper.LoadImage(_indiagram.ImagePath, _pictureWidth, _pictureHeight);
                canvas.DrawBitmap(image, _marginLeft, _marginTop, _picturePainter);
            }
            catch (Exception)
            {
                canvas.DrawRect(_marginLeft, _marginTop, _pictureWidth + _marginLeft, _pictureHeight + _marginTop, _picturePainter);
            }

            if (!string.IsNullOrEmpty(_indiagram.Text))
            {
                //write text
                int yindex = _marginTop + _pictureHeight + SettingsService.FontSize;
                int xindex = _marginLeft + _pictureWidth / 2;
                String text = _indiagram.Text;

                if (_isOneLineText)
                {
                    canvas.DrawText(text, xindex, yindex, _textPainter);
                }
                else
                {
                    int txtOffset = 0;

                    while (txtOffset < text.Length)
                    {
                        int textSize = _textPainter.BreakText(text, txtOffset, text.Length, true, _pictureWidth, null);
                        string text2 = text.Substring(txtOffset, textSize);

                        canvas.DrawText(text2, xindex, yindex, _textPainter);

                        yindex += SettingsService.FontSize;
                        txtOffset += textSize;
                    }
                }
            }
        }

        public static int DefaultWidth
        {
            get
            {
                return (int)(SettingsService.IndiagramDisplaySize * 1.2);
            }
        }

        public static int DefaultHeight
        {
            get
            {
                return (int)(SettingsService.IndiagramDisplaySize * 1.2 + SettingsService.FontSize);
            }
        }
    }
}