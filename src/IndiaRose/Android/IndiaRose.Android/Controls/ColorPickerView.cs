using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace IndiaRose.Droid.Controls
{
	internal class ColorPickerView : View
	{
		public event EventHandler<int> OnColorChanged;

		private const int PANEL_SAT_VAL = 0;
		private const int PANEL_HUE = 1;
		private const int PANEL_ALPHA = 2;
		private const float BORDER_WIDTH_PX = 1;
		private static float _mDensity = 1f;

		private float _huePanelWidth = 30f;
		private float _alphaPanelHeight = 20f;
		private float _panelSpacing = 10f;
		private float _paletteCircleTrackerRadius = 5f;
		private float _rectangleTrackerOffset = 2f;

		private Paint _satValPaint;
		private Paint _satValTrackerPaint;
		private Paint _huePaint;
		private Paint _hueAlphaTrackerPaint;
		private Paint _alphaTextPaint;
		private Paint _borderPaint;
		private Shader _valShader;
		private Shader _satShader;
		private Shader _hueShader;

		/*
		 * We cache a bitmap of the sat/val panel which is expensive to draw each time.
		 * We can reuse it when the user is sliding the circle picker as long as the hue isn't changed.
		 */
		private BitmapCache _satValBackgroundCache;
		private int _alpha = 0xff;
		private float _hue = 360f;
		private float _sat;
		private float _val;
		private string _alphaSliderText;
		private uint _sliderTrackerColor = 0xFFBDBDBD;
		private uint _borderColor = 0xFF6E6E6E;
		private bool _showAlphaPanel;
		/*
		 * To remember which panel that has the "focus" when 
		 * processing hardware button data.
		 */
		private int _lastTouchedPanel = PANEL_SAT_VAL;
		/**
		 * Offset from the edge we must have or else
		 * the finger tracker will get clipped when
		 * it is drawn outside of the view.
		 */
		private int _drawingOffset;
		/*
		 * Distance form the edges of the view 
		 * of where we are allowed to draw.
		 */
		private RectF _drawingRect;
		private RectF _satValRect;
		private RectF _hueRect;
		private RectF _alphaRect;
		private Point _startTouchPoint;

		public ColorPickerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Init(null);
		}

		public ColorPickerView(Context context) : base(context)
		{
			Init(null);
		}

		public ColorPickerView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(attrs);
		}

		public ColorPickerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(attrs);
		}

		public ColorPickerView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(attrs);
		}

		protected void RaiseColorChanged(int newColor)
		{
			OnColorChanged?.Invoke(this, newColor);
		}

		// ReSharper disable once UnusedParameter.Local
		private void Init(IAttributeSet attrs)
		{

			_showAlphaPanel = false;
			_alphaSliderText = "";
			_sliderTrackerColor = 0xFFBDBDBD;
			_borderColor = 0xFF6E6E6E;


			_mDensity = Context.Resources.DisplayMetrics.Density;
			_paletteCircleTrackerRadius *= _mDensity;
			_rectangleTrackerOffset *= _mDensity;
			_huePanelWidth *= _mDensity;
			_alphaPanelHeight *= _mDensity;
			_panelSpacing = _panelSpacing * _mDensity;

			_drawingOffset = CalculateRequiredOffset();

			InitPaintTools();

			Focusable = true;
			FocusableInTouchMode = true;
		}

		private void InitPaintTools()
		{

			_satValPaint = new Paint();
			_satValTrackerPaint = new Paint();
			_huePaint = new Paint();
			_hueAlphaTrackerPaint = new Paint();
			_alphaTextPaint = new Paint();
			_borderPaint = new Paint();


			_satValTrackerPaint.SetStyle(Paint.Style.Stroke);
			_satValTrackerPaint.StrokeWidth = 2f * _mDensity;
			_satValTrackerPaint.AntiAlias = true;

			_hueAlphaTrackerPaint.Color = new Color((int)_sliderTrackerColor);
			_hueAlphaTrackerPaint.SetStyle(Paint.Style.Stroke);
			_hueAlphaTrackerPaint.StrokeWidth = 2f * _mDensity;
			_hueAlphaTrackerPaint.AntiAlias = true;

			uint colorValue = 0xFF1C1C1C;
			_alphaTextPaint.Color = new Color((int)colorValue);
			_alphaTextPaint.TextSize = 14f * _mDensity;
			_alphaTextPaint.AntiAlias = true;
			_alphaTextPaint.TextAlign = Paint.Align.Center;
			_alphaTextPaint.FakeBoldText = true;

		}

		private int CalculateRequiredOffset()
		{
			float offset = Math.Max(_paletteCircleTrackerRadius, _rectangleTrackerOffset);
			offset = Math.Max(offset, BORDER_WIDTH_PX * _mDensity);

			return (int)(offset * 1.5f);
		}

		private int[] BuildHueColorArray()
		{
			int[] hue = new int[361];

			int count = 0;
			for (int i = hue.Length - 1; i >= 0; i--, count++)
			{
				hue[count] = Color.HSVToColor(new[] { i, 1f, 1f });
			}

			return hue;
		}


		protected override void OnDraw(Canvas canvas)
		{
			if (_drawingRect.Width() <= 0 || _drawingRect.Height() <= 0)
			{
				return;
			}

			DrawSatValPanel(canvas);
			DrawHuePanel(canvas);
			//drawAlphaPanel(canvas);

		}

		private void DrawSatValPanel(Canvas canvas)
		{
			/*
			 * Draw time for this code without using bitmap cache and hardware acceleration was around 20ms.
			 * Now with the bitmap cache and the ability to use hardware acceleration we are down at 1ms as long as the hue isn't changed.
			 * If the hue is changed we the sat/val rectangle will be rendered in software and it takes around 10ms.
			 * But since the rest of the view will be rendered in hardware the performance gain is big!
			 */

			RectF rect = _satValRect;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (BORDER_WIDTH_PX > 0)
			{
				_borderPaint.Color = new Color((int)_borderColor);
				canvas.DrawRect(_drawingRect.Left, _drawingRect.Top, rect.Right + BORDER_WIDTH_PX, rect.Bottom + BORDER_WIDTH_PX, _borderPaint);
			}

			if (_valShader == null)
			{
				//Black gradient has either not been created or the view has been resized.			
				uint startColor = 0xFFFFFFFF;
				uint endColor = 0xFF000000;
				_valShader = new LinearGradient(rect.Left, rect.Top, rect.Left, rect.Bottom,
						new Color((int)startColor), new Color((int)endColor), Shader.TileMode.Clamp);
			}


			//If the hue has changed we need to recreate the cache.
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (_satValBackgroundCache == null || _satValBackgroundCache.Value != _hue)
			{

				if (_satValBackgroundCache == null)
				{
					_satValBackgroundCache = new BitmapCache();
				}

				//We create our bitmap in the cache if it doesn't exist.
				if (_satValBackgroundCache.Bitmap == null)
				{
					_satValBackgroundCache.Bitmap = Bitmap.CreateBitmap((int)rect.Width(), (int)rect.Height(), Bitmap.Config.Argb8888);
				}

				//We create the canvas once so we can draw on our bitmap and the hold on to it.
				if (_satValBackgroundCache.Canvas == null)
				{
					_satValBackgroundCache.Canvas = new Canvas(_satValBackgroundCache.Bitmap);
				}

				int rgb = Color.HSVToColor(new[] { _hue, 1f, 1f });
				uint startColor = 0xFFFFFFFF;
				_satShader = new LinearGradient(rect.Left, rect.Top, rect.Right, rect.Top,
						new Color((int)startColor), new Color(rgb), Shader.TileMode.Clamp);

				ComposeShader mShader = new ComposeShader(_valShader, _satShader, PorterDuff.Mode.Multiply);
				_satValPaint.SetShader(mShader);

				//ly we draw on our canvas, the result will be stored in our bitmap which is already in the cache.
				//Since this is drawn on a canvas not rendered on screen it will automatically not be using the hardware acceleration.
				//And this was the code that wasn't supported by hardware acceleration which mean there is no need to turn it of anymore.
				//The rest of the view will still be hardware accelerated!!
				_satValBackgroundCache.Canvas.DrawRect(0, 0, _satValBackgroundCache.Bitmap.Width, _satValBackgroundCache.Bitmap.Height, _satValPaint);

				//We set the hue value in our cache to which hue it was drawn with, 
				//then we know that if it hasn't changed we can reuse our cached bitmap.
				_satValBackgroundCache.Value = _hue;

			}

			//We draw our bitmap from the cached, if the hue has changed
			//then it was just recreated otherwise the old one will be used.
			canvas.DrawBitmap(_satValBackgroundCache.Bitmap, null, rect, null);


			Point p = SatValToPoint(_sat, _val);

			_satValTrackerPaint.Color = Color.Black;
			canvas.DrawCircle(p.X, p.Y, _paletteCircleTrackerRadius - 1f * _mDensity, _satValTrackerPaint);

			_satValTrackerPaint.Color = Color.LightGray;
			canvas.DrawCircle(p.X, p.Y, _paletteCircleTrackerRadius, _satValTrackerPaint);

		}

		private void DrawHuePanel(Canvas canvas)
		{
			/*
			 * Drawn with hw acceleration, very fast.
			 */

			//long start = SystemClock.elapsedRealtime();

			RectF rect = _hueRect;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (BORDER_WIDTH_PX > 0)
			{
				_borderPaint.Color = new Color((int)_borderColor);
				canvas.DrawRect(rect.Left - BORDER_WIDTH_PX,
						rect.Top - BORDER_WIDTH_PX,
						rect.Right + BORDER_WIDTH_PX,
						rect.Bottom + BORDER_WIDTH_PX,
						_borderPaint);
			}

			if (_hueShader == null)
			{
				//The hue shader has either not yet been created or the view has been resized.
				_hueShader = new LinearGradient(0, 0, 0, rect.Height(), BuildHueColorArray(), null, Shader.TileMode.Clamp);
				_huePaint.SetShader(_hueShader);
			}

			canvas.DrawRect(rect, _huePaint);

			float rectHeight = 4 * _mDensity / 2;

			Point p = HueToPoint(_hue);

			RectF r = new RectF
			{
				Left = rect.Left - _rectangleTrackerOffset,
				Right = rect.Right + _rectangleTrackerOffset,
				Top = p.Y - rectHeight,
				Bottom = p.Y + rectHeight
			};


			canvas.DrawRoundRect(r, 2, 2, _hueAlphaTrackerPaint);

			//Log.d("mColorPicker", "Draw Time Hue: " + (SystemClock.elapsedRealtime() - start) + "ms");

		}


		private Point HueToPoint(float hue)
		{

			RectF rect = _hueRect;
			float height = rect.Height();

			Point p = new Point { Y = (int)(height - (hue * height / 360f) + rect.Top), X = (int)rect.Left };

			return p;
		}

		private Point SatValToPoint(float sat, float val)
		{

			RectF rect = _satValRect;
			float height = rect.Height();
			float width = rect.Width();

			Point p = new Point
			{
				X = (int)(sat * width + rect.Left),
				Y = (int)((1f - val) * height + rect.Top)
			};


			return p;
		}

		private float[] PointToSatVal(float x, float y)
		{

			RectF rect = _satValRect;
			float[] result = new float[2];

			float width = rect.Width();
			float height = rect.Height();

			if (x < rect.Left)
			{
				x = 0f;
			}
			else if (x > rect.Right)
			{
				x = width;
			}
			else
			{
				x = x - rect.Left;
			}

			if (y < rect.Top)
			{
				y = 0f;
			}
			else if (y > rect.Bottom)
			{
				y = height;
			}
			else
			{
				y = y - rect.Top;
			}


			result[0] = 1f / width * x;
			result[1] = 1f - (1f / height * y);

			return result;
		}

		private float PointToHue(float y)
		{

			RectF rect = _hueRect;

			float height = rect.Height();

			if (y < rect.Top)
			{
				y = 0f;
			}
			else if (y > rect.Bottom)
			{
				y = height;
			}
			else
			{
				y = y - rect.Top;
			}

			return 360f - (y * 360f / height);
		}

		private int PointToAlpha(int x)
		{

			RectF rect = _alphaRect;
			int width = (int)rect.Width();

			if (x < rect.Left)
			{
				x = 0;
			}
			else if (x > rect.Right)
			{
				x = width;
			}
			else
			{
				x = x - (int)rect.Left;
			}

			return 0xff - (x * 0xff / width);

		}


		public override bool OnTrackballEvent(MotionEvent e)
		{
			float x = e.GetX();
			float y = e.GetY();

			bool update = false;

			if (e.Action == MotionEventActions.Move)
			{

				switch (_lastTouchedPanel)
				{

					case PANEL_SAT_VAL:

						float sat = _sat + x / 50f;
						float val = _val - y / 50f;

						if (sat < 0f)
						{
							sat = 0f;
						}
						else if (sat > 1f)
						{
							sat = 1f;
						}

						if (val < 0f)
						{
							val = 0f;
						}
						else if (val > 1f)
						{
							val = 1f;
						}

						_sat = sat;
						_val = val;

						update = true;

						break;

					case PANEL_HUE:

						float hue = _hue - y * 10f;

						if (hue < 0f)
						{
							hue = 0f;
						}
						else if (hue > 360f)
						{
							hue = 360f;
						}

						_hue = hue;

						update = true;

						break;

					case PANEL_ALPHA:

						if (_showAlphaPanel && _alphaRect != null)
						{
							int alpha = (int)(_alpha - x * 10);

							if (alpha < 0)
							{
								alpha = 0;
							}
							else if (alpha > 0xff)
							{
								alpha = 0xff;
							}

							_alpha = alpha;


							update = true;
						}

						break;
				}


			}


			if (update)
			{
				Color color = Color.HSVToColor(_alpha, new[] { _hue, _sat, _val });
				RaiseColorChanged(color.ToArgb());

				Invalidate();
				return true;
			}


			return base.OnTrackballEvent(e);
		}

		private bool _hasFocus = false;
		public override bool OnTouchEvent(MotionEvent eventArgs)
		{
			bool update = false;

			switch (eventArgs.Action)
			{
				case MotionEventActions.Down:
					_hasFocus = true;
					Parent.RequestDisallowInterceptTouchEvent(true);
					_startTouchPoint = new Point((int)eventArgs.GetX(), (int)eventArgs.GetY());
					update = MoveTrackersIfNeeded(eventArgs);
					break;
				case MotionEventActions.Move:
					update = MoveTrackersIfNeeded(eventArgs);
					break;
				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
				case MotionEventActions.Outside:
					_startTouchPoint = null;
					update = MoveTrackersIfNeeded(eventArgs);
					_hasFocus = false;
					Parent.RequestDisallowInterceptTouchEvent(false);
					break;
			}
			
			if (update)
			{
				RaiseColorChanged(Color.HSVToColor(_alpha, new[] { _hue, _sat, _val }).ToArgb());
				Invalidate();
				return true;
			}

			return base.OnTouchEvent(eventArgs);
		}

		private bool MoveTrackersIfNeeded(MotionEvent eventArgs)
		{

			if (_startTouchPoint == null)
			{
				return false;
			}

			bool update = false;

			int startX = _startTouchPoint.X;
			int startY = _startTouchPoint.Y;


			if (_hueRect.Contains(startX, startY))
			{
				_lastTouchedPanel = PANEL_HUE;

				_hue = PointToHue(eventArgs.GetY());

				update = true;
			}
			else if (_satValRect.Contains(startX, startY))
			{

				_lastTouchedPanel = PANEL_SAT_VAL;

				float[] result = PointToSatVal(eventArgs.GetX(), eventArgs.GetY());

				_sat = result[0];
				_val = result[1];

				update = true;
			}
			else if (_alphaRect != null && _alphaRect.Contains(startX, startY))
			{

				_lastTouchedPanel = PANEL_ALPHA;

				_alpha = PointToAlpha((int)eventArgs.GetX());

				update = true;
			}


			return update;
		}


		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			int width = 0;
			int height = 0;

			MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);
			MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);

			int widthAllowed = MeasureSpec.GetSize(widthMeasureSpec);
			int heightAllowed = MeasureSpec.GetSize(heightMeasureSpec);


			//Log.d("color-picker-view", "widthMode: " + modeToString(widthMode) + " heightMode: " + modeToString(heightMode) + " widthAllowed: " + widthAllowed + " heightAllowed: " + heightAllowed);

			if (widthMode == MeasureSpecMode.Exactly || heightMode == MeasureSpecMode.Exactly)
			{
				//A exact value has been set in either direction, we need to stay within this size.

				if (widthMode == MeasureSpecMode.Exactly && heightMode != MeasureSpecMode.Exactly)
				{
					//The with has been specified exactly, we need to adopt the height to fit.
					int h = (int)(widthAllowed - _panelSpacing - _huePanelWidth);

					if (_showAlphaPanel)
					{
						h += (int)(_panelSpacing + _alphaPanelHeight);
					}

					height = h > heightAllowed ? heightAllowed : h;

					width = widthAllowed;

				}
				else if (heightMode == MeasureSpecMode.Exactly && widthMode != MeasureSpecMode.Exactly)
				{
					//The height has been specified exactly, we need to stay within this height and adopt the width.

					int w = (int)(heightAllowed + _panelSpacing + _huePanelWidth);

					if (_showAlphaPanel)
					{
						w -= (int)(_panelSpacing - _alphaPanelHeight);
					}

					width = w > widthAllowed ? widthAllowed : w;

					height = heightAllowed;

				}
				else
				{
					//If we get here the dev has set the width and height to exact sizes. For example match_parent or 300dp.
					//This will mean that the sat/val panel will not be square but it doesn't matter. It will work anyway.
					//In all other senarios our goal is to make that panel square.

					//We set the sizes to exactly what we were told.
					width = widthAllowed;
					height = heightAllowed;
				}

			}
			else
			{
				//If no exact size has been set we try to make our view as big as possible 
				//within the allowed space.

				//Calculate the needed with to layout the view based on the allowed height.
				int widthNeeded = (int)(heightAllowed + _panelSpacing + _huePanelWidth);
				//Calculate the needed height to layout the view based on the allowed width.
				int heightNeeded = (int)(widthAllowed - _panelSpacing - _huePanelWidth);

				if (_showAlphaPanel)
				{
					widthNeeded -= (int)(_panelSpacing + _alphaPanelHeight);
					heightNeeded += (int)(_panelSpacing + _alphaPanelHeight);
				}


				if (widthNeeded <= widthAllowed)
				{
					width = widthNeeded;
					height = heightAllowed;
				}
				else if (heightNeeded <= heightAllowed)
				{
					height = heightNeeded;
					width = widthAllowed;
				}
			}

			//Log.d("mColorPicker", " Size: " + Width + "x" + Height);

			SetMeasuredDimension(width, height);
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			_drawingRect = new RectF
			{
				Left = _drawingOffset + PaddingLeft,
				Right = w - _drawingOffset - PaddingRight,
				Top = _drawingOffset + PaddingTop,
				Bottom = h - _drawingOffset - PaddingBottom
			};

			//The need to be recreated because they depend on the size of the view.
			_valShader = null;
			_satShader = null;
			_hueShader = null;


			SetUpSatValRect();
			SetUpHueRect();
			SetUpAlphaRect();
		}

		private void SetUpSatValRect()
		{
			//Calculate the size for the big color rectangle.
			RectF dRect = _drawingRect;

			float left = dRect.Left + BORDER_WIDTH_PX;
			float top = dRect.Top + BORDER_WIDTH_PX;
			float bottom = dRect.Bottom - BORDER_WIDTH_PX;
			float right = dRect.Right - BORDER_WIDTH_PX - _panelSpacing - _huePanelWidth;


			if (_showAlphaPanel)
			{
				bottom -= (_alphaPanelHeight + _panelSpacing);
			}

			_satValRect = new RectF(left, top, right, bottom);
		}

		private void SetUpHueRect()
		{
			//Calculate the size for the hue slider on the left.
			RectF dRect = _drawingRect;

			float left = dRect.Right - _huePanelWidth + BORDER_WIDTH_PX;
			float top = dRect.Top + BORDER_WIDTH_PX;
			float bottom = dRect.Bottom - BORDER_WIDTH_PX - (_showAlphaPanel ? (_panelSpacing + _alphaPanelHeight) : 0);
			float right = dRect.Right - BORDER_WIDTH_PX;

			_hueRect = new RectF(left, top, right, bottom);
		}

		private void SetUpAlphaRect()
		{

			if (!_showAlphaPanel) return;

			RectF dRect = _drawingRect;

			float left = dRect.Left + BORDER_WIDTH_PX;
			float top = dRect.Bottom - _alphaPanelHeight + BORDER_WIDTH_PX;
			float bottom = dRect.Bottom - BORDER_WIDTH_PX;
			float right = dRect.Right - BORDER_WIDTH_PX;

			_alphaRect = new RectF(left, top, right, bottom);


			//mAlphaPattern = new AlphaPatternDrawable((int) (5 * mDensity));
			//mAlphaPattern.setBounds(Math.round(mAlphaRect.Left), Math
			//		.round(mAlphaRect.Top), Math.round(mAlphaRect.Right), Math
			//		.round(mAlphaRect.Bottom));
		}

		/**
		 * Get the current color this view is showing.
		 * @return the current color.
		 */
		public int GetColor()
		{
			return Color.HSVToColor(_alpha, new[] { _hue, _sat, _val });
		}

		/**
		 * Set the color the view should show.
		 * @param color The color that should be selected.
		 */
		public void SetColor(int color)
		{
			SetColor(color, false);
		}

		/**
		 * Set the color this view should show.
		 * @param color The color that should be selected.
		 * @param callback If you want to get a callback to
		 * your OnColorChangedListener.
		 */
		public void SetColor(int color, bool callback)
		{
			Color c = new Color(color);
			int alpha = c.A;
			int red = c.R;
			int blue = c.B;
			int green = c.G;

			float[] hsv = new float[3];

			Color.RGBToHSV(red, green, blue, hsv);

			_alpha = alpha;
			_hue = hsv[0];
			_sat = hsv[1];
			_val = hsv[2];

			if (callback)
			{
				RaiseColorChanged(Color.HSVToColor(_alpha, new[] { _hue, _sat, _val }).ToArgb());
			}

			Invalidate();
		}

		/**
		 * Get the drawing offset of the color picker view.
		 * The drawing offset is the distance from the side of
		 * a panel to the side of the view minus the padding.
		 * Useful if you want to have your own panel below showing
		 * the currently selected color and want to align it perfectly.
		 * @return The offset in pixels.
		 */
		public float GetDrawingOffset()
		{
			return _drawingOffset;
		}

		/**
		 * Set if the user is allowed to adjust the alpha panel. Default is false.
		 * If it is set to false no alpha will be set.
		 * @param visible
		 */
		public void SetAlphaSliderVisible(bool visible)
		{

			if (_showAlphaPanel != visible)
			{
				_showAlphaPanel = visible;

				/*
				 * Reset all shader to force a recreation. 
				 * Otherwise they will not look right after 
				 * the size of the view has changed.
				 */
				_valShader = null;
				_satShader = null;
				_hueShader = null;


				RequestLayout();
			}

		}

		/**
		 * Set the color of the tracker slider on the hue and alpha panel.
		 * @param color
		 */
		public void SetSliderTrackerColor(uint color)
		{
			_sliderTrackerColor = color;
			_hueAlphaTrackerPaint.Color = new Color((int)_sliderTrackerColor);
			Invalidate();
		}

		/**
		 * Get color of the tracker slider on the hue and alpha panel.
		 * @return
		 */
		public int GetSliderTrackerColor()
		{
			return (int)_sliderTrackerColor;
		}

		/**
		 * Set the color of the border surrounding all panels.
		 * @param color
		 */
		public void SetBorderColor(uint color)
		{
			_borderColor = color;
			Invalidate();
		}

		/**
		 * Get the color of the border surrounding all panels.
		 */
		public int GetBorderColor()
		{
			return (int)_borderColor;
		}


		/**
		 * Set the text that should be shown in the 
		 * alpha slider. Set to null to disable text.
		 * @param text Text that should be shown.
		 */
		public void SetAlphaSliderText(string text)
		{
			_alphaSliderText = text;
			Invalidate();
		}

		/**
		 * Get the current value of the text
		 * that will be shown in the alpha
		 * slider.
		 * @return
		 */
		public string GetAlphaSliderText()
		{
			return _alphaSliderText;
		}

		private class BitmapCache
		{
			public Canvas Canvas;
			public Bitmap Bitmap;
			public float Value;
		}
	}
}