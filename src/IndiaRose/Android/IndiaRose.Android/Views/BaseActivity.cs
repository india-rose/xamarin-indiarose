using Android.App;
using Android.OS;
using ReactiveUI;

namespace IndiaRose.Droid.Views
{
	public abstract class BaseActivity<TViewModel> : ReactiveActivity<TViewModel> where TViewModel : class
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

			BindControls();
		}

		protected virtual void BindControls()
		{

		}
	}
}