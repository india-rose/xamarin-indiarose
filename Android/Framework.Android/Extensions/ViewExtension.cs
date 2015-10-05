using System;
using Android.Views;

namespace IndiaRose.Framework.Extensions
{
	public static class ViewExtension
	{
		public static void AttachOnLayoutChanged(this View view, Action onLayoutChange)
		{
			new ViewLayoutChangeHandler(view, onLayoutChange);
		}
	}

	public class ViewLayoutChangeHandler
	{
		private Action _onLayoutChange;
		private readonly View _attachedView;

		public ViewLayoutChangeHandler(View view, Action onLayoutChange)
		{
			_attachedView = view;
			_onLayoutChange = onLayoutChange;

			_attachedView.ViewTreeObserver.GlobalLayout += OnLayoutChange;
		}

		private void OnLayoutChange(object sender, EventArgs eventArgs)
		{
			_attachedView.ViewTreeObserver.GlobalLayout -= OnLayoutChange;

			Action callback = _onLayoutChange;
			_onLayoutChange = null;
			if (callback != null)
			{
				callback();
			}
		}
	}
}