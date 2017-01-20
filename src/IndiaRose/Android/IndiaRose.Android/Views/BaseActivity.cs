using Android.App;
using Android.OS;
using IndiaRose.Droid.Helpers;
using ReactiveUI;
using ReactiveUI.AndroidSupport;

namespace IndiaRose.Droid.Views
{
	public abstract class BaseActivity<TViewModel> : ReactiveAppCompatActivity<TViewModel> where TViewModel : class
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

			if (savedInstanceState == null)
			{
				ViewModel = ViewModel ?? CreateViewModel();
				CreateFragments();
			}
			else
			{
				ViewModel = ViewModel ?? BundleSave.Get<TViewModel>(savedInstanceState.GetGuid(GetType().FullName)) ?? CreateViewModel();
				RestoreFragments();
			}
		}

		protected virtual void CreateFragments()
		{

		}

		protected virtual void RestoreFragments()
		{

		}

		protected virtual void BindControls()
		{

		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);

			outState.SetGuid(GetType().FullName, BundleSave.Save(ViewModel));
		}

		protected abstract TViewModel CreateViewModel();
	}
}