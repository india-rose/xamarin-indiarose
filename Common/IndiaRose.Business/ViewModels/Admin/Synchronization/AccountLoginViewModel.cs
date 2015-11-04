using System.Windows.Input;
using Storm.Mvvm.Commands;

namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel
	{
		private string _inputLogin;
		private string _inputPassword;

		private bool _inputRememberMe;

		public string InputLogin
		{
			get { return _inputLogin; }
			set { SetProperty(ref _inputLogin, value); }
		}

		public string InputPassword
		{
			get { return _inputPassword; }
			set { SetProperty(ref _inputPassword, value); }
		}

		public bool InputRememberMe
		{
			get { return _inputRememberMe; }
			set { SetProperty(ref _inputRememberMe, value); }
		}

		public ICommand LoginCommand { get; private set; }
		public ICommand OpenRegisterCommand { get; private set; }

		public void AccountLoginInit()
		{
			LoginCommand = new DelegateCommand(LoginAction);
			OpenRegisterCommand = new DelegateCommand(OpenRegisterAction);
		}

		partial void OnNavigatedToAccountLogin()
		{
			InputLogin = SynchronizationSettingsService.UserLogin;
			InputPassword = "";
			InputRememberMe = SynchronizationSettingsService.UserRememberMe;
		}

		private async void LoginAction()
		{
			if (string.IsNullOrEmpty(InputLogin) || string.IsNullOrEmpty(InputPassword))
			{
				SetError(LocalizationService.GetString("AccountLogin_EmptyLoginOrPassword", "Text"));
				return;
			}

			if (await ConnectUserAsync(InputLogin, HashPassword(InputPassword)))
			{
				if (!InputLogin.Equals(SynchronizationSettingsService.UserLogin))
				{
					SynchronizationSettingsService.UserLogin = InputLogin;
				}

				if (InputRememberMe)
				{
					SynchronizationSettingsService.UserPasswd = InputPassword;
				}
				await SynchronizationSettingsService.SaveAsync();

				if (await SelectDeviceAsync(SynchronizationSettingsService.DeviceName))
				{
					TransitionToState(SynchronizationPageState.SummaryPage);
				}
				else
				{
					TransitionToState(SynchronizationPageState.DeviceChoose);
				}
			}
			else
			{
				SetError(LocalizationService.GetString("AccountLogin_InvalidCredentials", "Text"));
			}
		}

		private void OpenRegisterAction()
		{
			TransitionToState(SynchronizationPageState.AccountRegister);
		}
	}
}
