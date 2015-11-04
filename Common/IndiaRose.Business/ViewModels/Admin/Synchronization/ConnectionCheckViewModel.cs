namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel
	{
		private string _connectionStatusMessage;

		public string ConnectionStatusMessage
		{
			get { return _connectionStatusMessage; }
			set { SetProperty(ref _connectionStatusMessage, value); }
		}

		async partial void OnNavigatedToConnecting()
		{
			ConnectionStatusMessage = LocalizationService.GetString("ConnectionCheck_Connecting", "Text");
			
			bool result = await ApiService.IsAlive();

			if (result)
			{
				// check auth

			}
			else
			{
				// error webservice could be offline or no internet connection available
				ConnectionStatusMessage = LocalizationService.GetString("ConnectionCheck_ErrorNetwork", "Text");
			}
		}
	}
}
