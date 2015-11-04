using IndiaRose.WebAPI.Sdk.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel : AbstractViewModel
	{
		public enum SynchronizationPageState
		{
			Starting,
			Connecting,
			AccountLogin,
			AccountRegister,
			DeviceChoose,
			SummaryPage,
		}

		private SynchronizationPageState _pageState = SynchronizationPageState.Starting;

		protected IApiService ApiService
		{
			get { return LazyResolver<IApiService>.Service; }
		}
		
		public SynchronizationPageState PageState
		{
			get { return _pageState; }
			set { SetProperty(ref _pageState, value); }
		}

		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			base.OnNavigatedTo(e, parametersKey);

			TransitionToState(SynchronizationPageState.Connecting);
		}

		private void TransitionToState(SynchronizationPageState state)
		{
			if (PageState == state)
			{
				return;
			}

			PageState = state;

			switch (state)
			{
				case SynchronizationPageState.Connecting:
					OnNavigatedToConnecting();
					break;
				case SynchronizationPageState.AccountLogin:
					OnNavigatedToAccountLogin();
					break;
				case SynchronizationPageState.AccountRegister:
					OnNavigatedToAccountRegister();
					break;
				case SynchronizationPageState.DeviceChoose:
					OnNavigatedToDeviceChoose();
					break;
				case SynchronizationPageState.SummaryPage:
					OnNavigatedToSummaryPage();
					break;
			}
		}

		partial void OnNavigatedToConnecting();
		partial void OnNavigatedToAccountLogin();
		partial void OnNavigatedToAccountRegister();
		partial void OnNavigatedToDeviceChoose();
		partial void OnNavigatedToSummaryPage();
	}
}