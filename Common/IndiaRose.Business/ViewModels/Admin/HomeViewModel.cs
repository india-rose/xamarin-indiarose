#region Usings

using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin
{
	public class HomeViewModel : ViewModelBase
	{
		public ICommand ExitCommand { get; private set; }
		public ICommand CreditsCommand { get; private set; }
        public ICommand CollectionCommand { get; private set; }
		public ICommand HelpCommand { get; private set; }
		public ICommand ContactCommand { get; private set; }
		public ICommand TtsCommand { get; private set; }
		public ICommand SettingsCommand { get; private set; }
		public ICommand SynchroCommand { get; private set; }

		public HomeViewModel(IContainer container) : base(container)
		{
			ExitCommand = new DelegateCommand(ExitAction);
			CreditsCommand = new DelegateCommand(CreditsAction);
            CollectionCommand = new DelegateCommand(CollectionAction);
			HelpCommand = new DelegateCommand(HelpAction);
			ContactCommand = new DelegateCommand(ContactAction);
			TtsCommand = new DelegateCommand(TTSAction);
			SettingsCommand = new DelegateCommand(SettingsAction);
			SynchroCommand = new DelegateCommand(SynchroAction);
		}

		private void SettingsAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_APPSETTINGS);
		}

		private void SynchroAction()
		{
			NavigationService.Navigate(Views.ADMIN_SERVERSYNCHRONIZATION);
		}

		private void TTSAction()
		{
			NavigationService.Navigate(Views.ADMIN_INSTALLTTS);
		}

		private void ContactAction()
		{
			string subject = Resources.EMAIL_SUBJECT;
            string address = Resources.EMAIL_ADDR;
			string body = Resources.EMAIL_TEXT;
			Container.Resolve<IEmailService>().send(subject, address, body);
		}

		private void HelpAction()
		{
			Container.Resolve<IResourcesService>().Show(Resources.HELP);
		}

		private void CreditsAction()
		{
			NavigationService.Navigate(Views.ADMIN_CREDITS);
		}
        private void CollectionAction()
        {
            NavigationService.Navigate(Views.ADMIN_COLLECTION_MANAGEMENT);

        }

		private void ExitAction()
		{
			NavigationService.ExitApplication();
		}
	}
}