using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace IndiaRose.Droid.Controls
{
	//inspired by https://github.com/emilsjolander/StickyScrollViewItems
	public class StickyScrollView : ScrollView
	{
		public const string TAG_STICKY = "sticky";

		private List<View> _stickyViews;
		private View _currentStickedView;
		private float _stickyViewTopOffset;
		private float _stickyViewLeftOffset;
		private bool _redirectTouchToStickyView;
		private bool _clipToPadding;
		private bool _clipToPaddingHasBeenSetSet;

		private int _shadowHeight;
		private Drawable _shadowDrawable;
		private bool _downActionDone;

		#region Constructors

		public StickyScrollView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		public StickyScrollView(Context context) : base(context)
		{
			Initialize();
		}

		public StickyScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();
		}

		public StickyScrollView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		public StickyScrollView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Initialize();
		}

		#endregion

		private void Initialize()
		{
			_stickyViews = new List<View>();
		}

		private int LeftForViewRelativeOnlyChild(View v) => PaddingForViewRelativeOnlyChild(v, x => x.Left);

		private int TopForViewRelativeOnlyChild(View v) => PaddingForViewRelativeOnlyChild(v, x => x.Top);

		private int RightForViewRelativeOnlyChild(View v) => PaddingForViewRelativeOnlyChild(v, x => x.Right);

		private int BottomForViewRelativeOnlyChild(View v) => PaddingForViewRelativeOnlyChild(v, x => x.Bottom);

		private int PaddingForViewRelativeOnlyChild(View v, Func<View, int> paddingGetter)
		{
			int dimension = paddingGetter(v);
			View child = GetChildAt(0);
			while (v.Parent != child)
			{
				v = (View)v.Parent;
				dimension += paddingGetter(v);
			}
			return dimension;
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);

			if (!_clipToPaddingHasBeenSetSet)
			{
				_clipToPadding = true;
			}
			NotifyHierarchyChanged();
		}

		public override void SetClipToPadding(bool clipToPadding)
		{
			base.SetClipToPadding(clipToPadding);
			_clipToPadding = clipToPadding;
			_clipToPaddingHasBeenSetSet = true;
		}

		#region AddView override

		public override void AddView(View child)
		{
			base.AddView(child);
			FindStickyViews(child);
		}

		public override void AddView(View child, ViewGroup.LayoutParams @params)
		{
			base.AddView(child, @params);
			FindStickyViews(child);
		}

		public override void AddView(View child, int index)
		{
			base.AddView(child, index);
			FindStickyViews(child);
		}

		public override void AddView(View child, int index, ViewGroup.LayoutParams @params)
		{
			base.AddView(child, index, @params);
			FindStickyViews(child);
		}

		public override void AddView(View child, int width, int height)
		{
			base.AddView(child, width, height);
			FindStickyViews(child);
		}

		#endregion

		protected override void DispatchDraw(Canvas canvas)
		{
			base.DispatchDraw(canvas);
			if (_currentStickedView != null)
			{
				canvas.Save();
				canvas.Translate(PaddingLeft + _stickyViewLeftOffset, ScrollY + _stickyViewTopOffset + (_clipToPadding ? PaddingTop : 0));

				canvas.ClipRect(0, _clipToPadding ? -_stickyViewTopOffset : 0,
						  Width - _stickyViewLeftOffset,
						  _currentStickedView.Height + _shadowHeight + 1);

				if (_shadowDrawable != null)
				{
					int left = 0;
					int right = _currentStickedView.Width;
					int top = _currentStickedView.Height;
					int bottom = _currentStickedView.Height + _shadowHeight;
					_shadowDrawable.SetBounds(left, top, right, bottom);
					_shadowDrawable.Draw(canvas);
				}

				canvas.ClipRect(0, (_clipToPadding ? -_stickyViewTopOffset : 0), Width, _currentStickedView.Height);
				_currentStickedView.Draw(canvas);
				/*
				if (TagForView(_currentStickedView).contains(FLAG_HASTRANSPARANCY))
				{
					showView(currentlyStickingView);
					currentlyStickingView.draw(canvas);
					hideView(currentlyStickingView);
				}
				else
				{
					currentlyStickingView.draw(canvas);
				}
				*/
				canvas.Restore();
			}
		}

		public override bool DispatchTouchEvent(MotionEvent e)
		{
			if (e.Action == MotionEventActions.Down)
			{
				_redirectTouchToStickyView = true;
			}

			if (_currentStickedView == null)
			{
				_redirectTouchToStickyView = false;
			}
			else if (_redirectTouchToStickyView)
			{
				_redirectTouchToStickyView = e.GetY() <= _currentStickedView.Height + _stickyViewTopOffset
											&& e.GetX() >= LeftForViewRelativeOnlyChild(_currentStickedView)
											&& e.GetX() <= RightForViewRelativeOnlyChild(_currentStickedView);
			}
			if (_redirectTouchToStickyView)
			{
				e.OffsetLocation(0, -1 * ((ScrollY + _stickyViewTopOffset) - TopForViewRelativeOnlyChild(_currentStickedView)));
			}

			return base.DispatchTouchEvent(e);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			if (_redirectTouchToStickyView)
			{
				e.OffsetLocation(0, -1 * ((ScrollY + _stickyViewTopOffset) - TopForViewRelativeOnlyChild(_currentStickedView)));
			}

			if (e.Action == MotionEventActions.Down)
			{
				_downActionDone = true;
			}

			if (!_downActionDone)
			{
				MotionEvent downEvent = MotionEvent.Obtain(e);
				downEvent.Action = MotionEventActions.Down;
				base.OnTouchEvent(downEvent);
				_downActionDone = true;
			}

			if (e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Cancel)
			{
				_downActionDone = false;
			}

			return base.OnTouchEvent(e);
		}

		protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
		{
			base.OnScrollChanged(l, t, oldl, oldt);
			DoTheStickyThing();
		}

		private void DoTheStickyThing()
		{
			View viewThatShouldStick = null;
			int viewThatShouldStickTop = 0;
			View approachingView = null;
			int approachingViewTop = 0;
			foreach (View v in _stickyViews)
			{
				int viewTop = TopForViewRelativeOnlyChild(v) - ScrollY + (_clipToPadding ? 0 : PaddingTop);
				if (viewTop <= 0)
				{
					if (viewThatShouldStick == null || viewTop > viewThatShouldStickTop)
					{
						viewThatShouldStickTop = viewTop;
						viewThatShouldStick = v;
					}
				}
				else
				{
					if (approachingView == null || viewTop < approachingViewTop)
					{
						approachingViewTop = viewTop;
						approachingView = v;
					}
				}
			}
			if (viewThatShouldStick != null)
			{
				_stickyViewTopOffset = approachingView == null ? 0 : Math.Min(0, approachingViewTop - viewThatShouldStick.Height);
				if (viewThatShouldStick != _currentStickedView)
				{
					if (_currentStickedView != null)
					{
						StopStickingCurrentlyStickedView();
					}
					// only compute the left offset when we start sticking.
					_stickyViewLeftOffset = LeftForViewRelativeOnlyChild(viewThatShouldStick);
					StartStickingView(viewThatShouldStick);
				}
			}
			else if (_currentStickedView != null)
			{
				StopStickingCurrentlyStickedView();
			}
		}

		private void StartStickingView(View view)
		{
			_currentStickedView = view;
			/*
			if (TagForView(currentlyStickingView).contains(FLAG_HASTRANSPARANCY))
			{
				hideView(currentlyStickingView);
			}
			if (((String)currentlyStickingView.getTag()).contains(FLAG_NONCONSTANT))
			{
				post(invalidateRunnable);
			}
			*/
		}

		private void StopStickingCurrentlyStickedView()
		{
			/*
			if (getStringTagForView(currentlyStickingView).contains(FLAG_HASTRANSPARANCY))
			{
				showView(currentlyStickingView);
			}
			*/
			_currentStickedView = null;
			//RemoveCallbacks(invalidateRunnable);
		}

		public void NotifyStickyAttributeChanged()
		{
			NotifyHierarchyChanged();
		}

		private void NotifyHierarchyChanged()
		{
			if (_currentStickedView != null)
			{
				StopStickingCurrentlyStickedView();
			}
			_stickyViews.Clear();
			FindStickyViews(GetChildAt(0));
			DoTheStickyThing();
			Invalidate();
		}

		private void FindStickyViews(View v)
		{
			ViewGroup group = v as ViewGroup;
			if (group != null)
			{
				for (int i = 0; i < group.ChildCount; i++)
				{
					View child = group.GetChildAt(i);
					if(TagForView(v).Contains(TAG_STICKY))
					{
						_stickyViews.Add(child);
					}
					else if (child is ViewGroup)
					{
						FindStickyViews(child);
					}
				}
			}
			//below else part seems to be useless ?
			else
			{
				if (TagForView(v).Contains(TAG_STICKY))
				{
					_stickyViews.Add(v);
				}
			}
		}

		private string TagForView(View v) => v.Tag?.ToString() ?? "";

		private void HideView(View v)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.GingerbreadMr1)
			{
				v.Alpha = 0;
			}
			else
			{
				AlphaAnimation anim = new AlphaAnimation(1, 0)
				{
					Duration = 0,
					FillAfter = true
				};
				v.StartAnimation(anim);
			}
		}

		private void ShowView(View v)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
			{
				v.Alpha = 1;
			}
			else
			{
				AlphaAnimation anim = new AlphaAnimation(0, 1)
				{
					Duration = 0,
					FillAfter = true
				};
				v.StartAnimation(anim);
			}
		}
	}
}