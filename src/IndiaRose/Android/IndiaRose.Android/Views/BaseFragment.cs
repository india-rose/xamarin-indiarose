using Android.OS;
using Android.Views;
using ReactiveUI;

namespace IndiaRose.Droid.Views
{
	public abstract class BaseFragment<TViewModel> : ReactiveFragment where TViewModel : class
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

			BindControls();

			return RootView;
		}

		protected virtual void BindControls()
		{

		}
	}
}