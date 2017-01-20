using Android.OS;
using Android.Views;
using IndiaRose.Droid.Helpers;
using ReactiveUI.AndroidSupport;

namespace IndiaRose.Droid.Views
{
	public abstract class BaseFragment<TViewModel> : ReactiveFragment<TViewModel> where TViewModel : class
	{
		private readonly int _layoutId;
		protected View RootView { get; private set; }

		protected BaseFragment(int layoutId)
		{
			_layoutId = layoutId;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			RootView = inflater.Inflate(_layoutId, container, false);
			
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
			BindControls();

			return RootView;
		}

		protected virtual void RestoreFragments()
		{
			
		}

		protected virtual void CreateFragments()
		{
			
		}

		protected virtual void BindControls()
		{

		}

		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);

			outState.SetGuid(GetType().FullName, BundleSave.Save(ViewModel));
		}

		protected abstract TViewModel CreateViewModel();
	}
}