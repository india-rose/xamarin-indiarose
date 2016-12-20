using Android.App;
using Android.OS;

namespace IndiaRose.Droid.Views
{
	public abstract class BaseActivity : Activity
	{
		private readonly int _layoutId;

		protected BaseActivity(int layoutId)
		{
			_layoutId = layoutId;
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(_layoutId);
		}
	}
}