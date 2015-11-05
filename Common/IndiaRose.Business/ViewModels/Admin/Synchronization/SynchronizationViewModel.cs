using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using IndiaRose.WebAPI.Sdk.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel : AbstractViewModel
	{
		#region Services

		protected IApiService ApiService
		{
			get { return LazyResolver<IApiService>.Service; }
		}

		protected ISynchronizationSettingsService SynchronizationSettingsService
		{
			get { return LazyResolver<ISynchronizationSettingsService>.Service; }
		}

		#endregion

		#region Binding properties

		private SynchronizationPageState _pageState = SynchronizationPageState.Starting;
		private string _errorMessage;
		private bool _hasError;

		public SynchronizationPageState PageState
		{
			get { return _pageState; }
		    set
		    {
                LoggerService.Log("Set PageState to " + value, MessageSeverity.Info);
		        SetProperty(ref _pageState, value);
		    }
		}

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { SetProperty(ref _errorMessage, value); }
		}

		public bool HasError
		{
			get { return _hasError; }
			set { SetProperty(ref _hasError, value); }
		}

		#endregion

		public SynchronizationViewModel()
		{
			AccountLoginInit();
			AccountRegisterInit();
			DeviceChooseInit();
		}

		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			base.OnNavigatedTo(e, parametersKey);

			TransitionToState(SynchronizationPageState.Connecting);
		}

		private void TransitionToState(SynchronizationPageState state)
		{
			HasError = false;
			ErrorMessage = string.Empty;

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

		private void SetError(string error)
		{
			HasError = !string.IsNullOrEmpty(error);
			ErrorMessage = error;
		}

		partial void OnNavigatedToConnecting();
		partial void OnNavigatedToAccountLogin();
		partial void OnNavigatedToAccountRegister();
		partial void OnNavigatedToDeviceChoose();
		partial void OnNavigatedToSummaryPage();
	}
}