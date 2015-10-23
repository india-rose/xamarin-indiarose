using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IndiaRose.Framework.Views
{
	public class IndiagramPreviewView : RelativeLayout
	{
		private View _reinforcerView;
		private ImageView _indiagramView;

		private int _indiagramSize;
		private Color _reinforcerColor;
		private bool _reinforcerEnabled;

		public int IndiagramSize
		{
			get { return _indiagramSize; }
			set
			{
				if (_indiagramSize != value)
				{
					_indiagramSize = value;
					RefreshSize();
				}
			}
		}

		public Color ReinforcerColor
		{
			get { return _reinforcerColor; }
			set
			{
				if (!Equals(_reinforcerColor, value))
				{
					_reinforcerColor = value;
					RefreshColor();
				}
			}
		}

		public bool ReinforcerEnabled
		{
			get { return _reinforcerEnabled; }
			set
			{
				if (_reinforcerEnabled != value)
				{
					_reinforcerEnabled = value;
					RefreshColor();
				}
			}
		}

		private void RefreshSize()
		{
			ViewGroup.LayoutParams indiagramParam = _indiagramView.LayoutParameters;
			ViewGroup.LayoutParams reinforcerParam = _reinforcerView.LayoutParameters;

			indiagramParam.Height = IndiagramSize;
			indiagramParam.Width = IndiagramSize;

			reinforcerParam.Height = (int)(IndiagramSize * 1.2);
			reinforcerParam.Width = (int)(IndiagramSize * 1.2);

			Post(() =>
			{
				_reinforcerView.LayoutParameters = reinforcerParam;
				_indiagramView.LayoutParameters = indiagramParam;
				Invalidate();
			});
		}

		private void RefreshColor()
		{
			Post(() =>
			{
				_reinforcerView.SetBackgroundColor(_reinforcerEnabled ? _reinforcerColor : Color.Transparent);
				Invalidate();
			});
		}

		protected IndiagramPreviewView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		public IndiagramPreviewView(Context context) : base(context)
		{
			Initialize();
		}

		public IndiagramPreviewView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();
		}

		public IndiagramPreviewView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		private void Initialize()
		{
			_reinforcerView = new View(Context);
			LayoutParams param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			param.AddRule(LayoutRules.CenterInParent);
			AddView(_reinforcerView, param);

			_indiagramView = new ImageView(Context);
			param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			param.AddRule(LayoutRules.CenterInParent);
			AddView(_indiagramView, param);

			_indiagramView.SetImageResource(Resource.Drawable.root);
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);
			if (w != oldw || h != oldh)
			{
				RefreshSize();
			}
		}
	}
}