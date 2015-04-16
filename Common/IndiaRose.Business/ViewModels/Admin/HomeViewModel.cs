#region Usings

using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin
{
	public class HomeViewModel : ViewModelBase
	{
		private const string HELP_DOCUMENT_UID = "Help";
		private const string HELP_DOCUMENT_PROPERTY = "Document";

		#region Services

		private IEmailService _emailService;
		private IResourceService _resourceService;
		private ILocalizationService _localizationService;

		protected IEmailService EmailService
		{
			get { return _emailService ?? (_emailService = Container.Resolve<IEmailService>()); }
		}

		protected IResourceService ResourceService
		{
			get { return _resourceService ?? (_resourceService = Container.Resolve<IResourceService>()); }
		}

		protected ILocalizationService LocalizationService
		{
			get { return _localizationService ?? (_localizationService = Container.Resolve<ILocalizationService>()); }
		}

		#endregion

		#region Commands

		public ICommand SettingsCommand { get; private set; }
		public ICommand CollectionManagementCommand { get; private set; }

		public ICommand InstallVoiceSynthesisCommand { get; private set; }
		public ICommand SendLogsCommand { get; private set; }
		public ICommand SyncCollectionCommand { get; private set; }
		public ICommand HelpCommand { get; private set; }

		public ICommand ContactCommand { get; private set; }
		public ICommand ExitCommand { get; private set; }
		public ICommand CreditsCommand { get; private set; }

		#endregion
		
		public HomeViewModel(IContainer container) : base(container)
		{
			SettingsCommand = new DelegateCommand(SettingsAction);
            CollectionManagementCommand = new DelegateCommand(CollectionAction);
			
			InstallVoiceSynthesisCommand = new DelegateCommand(InstallVoiceSynthesisAction);
			SendLogsCommand = new DelegateCommand(SendLogAction);
			SyncCollectionCommand = new DelegateCommand(SyncCollectionAction);
			HelpCommand = new DelegateCommand(HelpAction);
			
			ContactCommand = new DelegateCommand(ContactAction);
			ExitCommand = new DelegateCommand(ExitAction);
			CreditsCommand = new DelegateCommand(CreditsAction);
		}

		#region First line command implementation

		private void SettingsAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_HOME);
		}

		private void CollectionAction()
		{
			NavigationService.Navigate(Views.ADMIN_COLLECTION_MANAGEMENT);
		}

		#endregion

		#region Second line command implementation

		private void InstallVoiceSynthesisAction()
		{
			NavigationService.Navigate(Views.ADMIN_INSTALLVOICE_SYNTHESIS);
		}

		private void SendLogAction()
		{
			//TODO : implement log sending
		}

		private void SyncCollectionAction()
		{
			NavigationService.Navigate(Views.ADMIN_SERVERSYNCHRONIZATION);
		}

		private void HelpAction()
		{
			string helpDocumentName = LocalizationService.GetString(HELP_DOCUMENT_UID, HELP_DOCUMENT_PROPERTY);

			if (string.IsNullOrWhiteSpace(helpDocumentName))
			{
				//TODO : implement message box to say no help
			}
			else
			{
				ResourceService.ShowPdfFile(helpDocumentName);
			}
		}

		#endregion

		#region Third line command implementation

		private void ContactAction()
		{
			if (!EmailService.SendContactEmail())
			{
				//TODO : implement message box to say unable to send email
			}
		}

		private void ExitAction()
		{
			NavigationService.ExitApplication();
		}

		private void CreditsAction()
		{
			NavigationService.Navigate(Views.ADMIN_CREDITS);
		}

		#endregion
	}
}