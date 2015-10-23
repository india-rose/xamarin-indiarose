using System;
using System.Windows.Input;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

#pragma warning disable 618

namespace IndiaRose.Framework.Views
{
	public class TabletPreviewView : AbsoluteLayout
	{
		private ImageView _tabletImageView;
		private ImageView _indiagramImageView;
		private ImageView _nextImageView;
		private ImageView _playImageView;

		private View _topArea;
		private View _bottomArea;

		private int _indiagramSize;
		private int _percentage;
		private Color _topAreaColor;
		private Color _bottomAreaColor;

		private int _width;
		private int _height;

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

		public int Percentage
		{
			get { return _percentage; }
			set
			{
				if (_percentage != value)
				{
					_percentage = value;
					RefreshSize();
				}
			}
		}

		public Color TopAreaColor
		{
			get { return _topAreaColor; }
			set
			{
				if (!Equals(_topAreaColor, value))
				{
					_topAreaColor = value;
					Post(() =>
					{
						_topArea.SetBackgroundColor(_topAreaColor);
						Invalidate();
					});
				}
			}
		}

		public Color BottomAreaColor
		{
			get { return _bottomAreaColor; }
			set
			{
				if (!Equals(_bottomAreaColor, value))
				{
					_bottomAreaColor = value;
					Post(() =>
					{
						_bottomArea.SetBackgroundColor(_bottomAreaColor);
						Invalidate();
					});
				}
			}
		}

		public ICommand TopAreaCommand { get; set; }
		public ICommand BottomAreaCommand { get; set; }

		protected IScreenService ScreenService
		{
			get
			{
				return LazyResolver<IScreenService>.Service;
			}
		}

		#region Constructors

		protected TabletPreviewView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
			Initialize();
		}

		public TabletPreviewView(Context context)
			: base(context)
		{
			Initialize();
		}

		public TabletPreviewView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			Initialize();
		}

		public TabletPreviewView(Context context, IAttributeSet attrs, int defStyleAttr)
			: base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		#endregion


		private void Initialize()
		{
			_tabletImageView = new ImageView(Context);
			_indiagramImageView = new ImageView(Context);
			_nextImageView = new ImageView(Context);
			_playImageView = new ImageView(Context);

			_tabletImageView.SetImageResource(Resource.Drawable.tab);
			_indiagramImageView.SetImageResource(Resource.Drawable.root);
			_nextImageView.SetImageResource(Resource.Drawable.rightarrow);
			_playImageView.SetImageResource(Resource.Drawable.playbutton);

			_topArea = new View(Context);
			_bottomArea = new View(Context);

			_topArea.Click += (sender, args) => ExecuteCommand(TopAreaCommand);
			_bottomArea.Click += (sender, args) => ExecuteCommand(BottomAreaCommand);

			foreach (View v in new[] { _topArea, _bottomArea, _indiagramImageView, _nextImageView, _playImageView, _tabletImageView })
			{
				AddView(v);
			}
		}

		private void ExecuteCommand(ICommand command)
		{
			if (command != null && command.CanExecute(null))
			{
				command.Execute(null);
			}
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
			if (_width > 0)
			{
				SetMeasuredDimension(_width, _height);
			}
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			if (w != oldw || h != oldh)
			{
				RefreshSize();
			}
		}

		private const int FULL_WIDTH = 1657;
		private const int FULL_HEIGHT = 1013;
		private const int EMPTY_WIDTH = 1280;
		private const int EMPTY_HEIGHT = 800;
		private const int LEFT_OFFSET = 188;
		private const int TOP_OFFSET = 105;

		private void RefreshSize()
		{
			if (Width == 0)
			{
				return;
			}

			// get available size in width and height
			// calculate ratio with image width and height
			double widthRatio = Width / (double)FULL_WIDTH;
			double heightRatio = Height / (double)FULL_HEIGHT;

			// find the lowest ratio to get the limiting thing
			double ratio = Math.Min(widthRatio, heightRatio);

			// find the correct dimension and center it
			int imageWidth = _width = (int)(ratio * FULL_WIDTH);
			int imageHeight = _height = (int)(ratio * FULL_HEIGHT);

			// find left/top offset to center the view in it's parent
			int leftOffset = (Width - imageWidth) / 2;
			int topOffset = (Height - imageHeight) / 2;

			// set new position
			UpdatePosition(_tabletImageView, leftOffset, topOffset, imageWidth, imageHeight);

			// get new position and width of empty area
			int leftEmptyArea = (int)Math.Floor(LEFT_OFFSET * ratio + leftOffset);
			int topEmptyArea = (int)Math.Floor(TOP_OFFSET * ratio + topOffset);
			int widthEmptyArea = (int)Math.Ceiling(EMPTY_WIDTH * ratio);
			int heightEmptyArea = (int)Math.Ceiling(EMPTY_HEIGHT * ratio);

			// calculate ratio compare to screen size
			double ratioToRealScreen = heightEmptyArea / (double)ScreenService.Height;

			int indiagramSize = (int)(ratioToRealScreen * IndiagramSize);
			int indiagramTopMargin = (int)(heightEmptyArea * 0.05);
			int indiagramLeftMargin = (int)(widthEmptyArea * 0.05);

			// place first indiagram
			UpdatePosition(_indiagramImageView, leftEmptyArea + indiagramLeftMargin, topEmptyArea + indiagramTopMargin, indiagramSize, indiagramSize);
			UpdatePosition(_nextImageView, leftEmptyArea + widthEmptyArea - indiagramSize - indiagramLeftMargin, topEmptyArea + indiagramTopMargin, indiagramSize, indiagramSize);

			int topHeight = (int)Math.Floor(heightEmptyArea * Percentage / 100f);
			int bottomHeight = heightEmptyArea - topHeight;

			UpdatePosition(_topArea, leftEmptyArea, topEmptyArea, widthEmptyArea, topHeight);
			UpdatePosition(_bottomArea, leftEmptyArea, topEmptyArea + topHeight, widthEmptyArea, bottomHeight);

			int topPlayButton = topEmptyArea + topHeight + (bottomHeight - indiagramSize) / 2;
			UpdatePosition(_playImageView, leftEmptyArea + widthEmptyArea - indiagramSize - indiagramLeftMargin, topPlayButton, indiagramSize, indiagramSize);

			Measure(imageWidth, imageHeight);
		}

		private void UpdatePosition(View view, int left, int top, int width, int height)
		{
			LayoutParams param = new LayoutParams(width, height, left, top);
			view.LayoutParameters = param;
		}
	}
}