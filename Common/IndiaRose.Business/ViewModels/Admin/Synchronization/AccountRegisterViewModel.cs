using System.Windows.Input;
using IndiaRose.Data.UIModel;
using IndiaRose.WebAPI.Sdk.Results;
using Storm.Mvvm.Commands;

namespace IndiaRose.Business.ViewModels.Admin.Synchronization
{
	public partial class SynchronizationViewModel
	{
		private string _inputEmail;
		private string _inputPasswordConfirmation;

		public string InputEmail
		{
			get { return _inputEmail; }
			set { SetProperty(ref _inputEmail, value); }
		}

		public string InputPasswordConfirmation
		{
			get { return _inputPasswordConfirmation; }
			set { SetProperty(ref _inputPasswordConfirmation, value); }
		}

		public ICommand CreateAccountCommand { get; private set; }
		public ICommand CancelAccountRegistrationCommand { get; private set; }

		public void AccountRegisterInit()
		{
			CreateAccountCommand = new DelegateCommand(CreateAccountAction);
			CancelAccountRegistrationCommand = new DelegateCommand(CancelAccountRegistrationAction);
		}

		partial void OnNavigatedToAccountRegister()
		{
			InputLogin = "";
			InputEmail = "";
			InputPassword = "";
			InputPasswordConfirmation = "";
			InputRememberMe = true;
		}

		private async void CreateAccountAction()
		{
			if (string.IsNullOrWhiteSpace(InputLogin) || string.IsNullOrWhiteSpace(InputPassword) || string.IsNullOrWhiteSpace(InputPasswordConfirmation) || string.IsNullOrWhiteSpace(InputEmail))
			{
				SetError(LocalizationService.GetString("AccountRegister_MissingField", "Text"));
				return;
			}

			if (!InputPassword.Equals(InputPasswordConfirmation))
			{
				SetError(LocalizationService.GetString("AccountRegister_InvalidPasswordConfirmation", "Text"));
				return;
			}

			UserStatusCode resultCode = await ApiService.RegisterUserAsync(InputLogin, InputEmail, InputPassword);
			if (resultCode != UserStatusCode.Ok)
			{
				string errorUid = "ServerError";
				switch (resultCode)
				{
					case UserStatusCode.EmailAlreadyExists:
						errorUid = "EmailAlreadyExists";
						break;
					case UserStatusCode.LoginAlreadyExists:
						errorUid = "LoginAlreadyExists";
						break;
				}
				SetError(LocalizationService.GetString(string.Format("AccountRegister_{0}", errorUid), "Text"));
				return;
			}

			SynchronizationSettingsService.Reset();
			SynchronizationSettingsService.UserLogin = InputLogin;
			SynchronizationSettingsService.UserRememberMe = InputRememberMe;
			if (InputRememberMe)
			{
				SynchronizationSettingsService.UserPasswd = HashPassword(InputPassword);
			}

			TransitionToState(SynchronizationPageState.DeviceChoose);
		}

		private void CancelAccountRegistrationAction()
		{
			TransitionToState(SynchronizationPageState.AccountLogin);
		}
	}
}
