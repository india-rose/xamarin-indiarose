using IndiaRose.Data.UIModel;

namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel
	{
		private string _connectionStatusMessage;
		private bool _isConnectionLoaderEnabled;

		public string ConnectionStatusMessage
		{
			get { return _connectionStatusMessage; }
			set { SetProperty(ref _connectionStatusMessage, value); }
		}

		public bool IsConnectionLoaderEnabled
		{
			get { return _isConnectionLoaderEnabled; }
			set { SetProperty(ref _isConnectionLoaderEnabled, value); }
		}

		async partial void OnNavigatedToConnecting()
		{
			IsConnectionLoaderEnabled = true;
			ConnectionStatusMessage = LocalizationService.GetString("ConnectionCheck_Connecting", "Text");

			bool result = await ApiService.IsAlive();

			if (result)
			{
				// check auth
				if (SynchronizationSettingsService.UserRememberMe)
				{
					if (await ConnectUserAsync(SynchronizationSettingsService.UserLogin, SynchronizationSettingsService.UserPasswd))
					{
						if (await SelectDeviceAsync(SynchronizationSettingsService.DeviceName))
						{
							TransitionToState(SynchronizationPageState.SummaryPage);
						}
						else
						{
							TransitionToState(SynchronizationPageState.DeviceChoose);
						}
						return;
					}
				}
				TransitionToState(SynchronizationPageState.AccountLogin);
			}
			else
			{
				IsConnectionLoaderEnabled = false;
				// error webservice could be offline or no internet connection available
				ConnectionStatusMessage = LocalizationService.GetString("ConnectionCheck_ErrorNetwork", "Text");
			}
		}
	}
}
