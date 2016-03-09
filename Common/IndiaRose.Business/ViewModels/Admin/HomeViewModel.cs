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
    /// <summary>
    /// VueModèle de la page d'accueil de la partie administrateur
    /// </summary>
	public class HomeViewModel : ViewModelBase
	{
		private const string HELP_DOCUMENT_UID = "Help";
		private const string HELP_DOCUMENT_PROPERTY = "Document";

		#region Services

		protected IEmailService EmailService
		{
			get { return LazyResolver<IEmailService>.Service; }
		}

		protected IResourceService ResourceService
		{
			get { return LazyResolver<IResourceService>.Service; }
		}

		protected ILocalizationService LocalizationService
		{
			get { return LazyResolver<ILocalizationService>.Service; }
		}

		protected IMessageDialogService MessageDialogService
		{
			get { return LazyResolver<IMessageDialogService>.Service; }
		}

		protected ICollectionStorageService CollectionStorageService
		{
			get { return LazyResolver<ICollectionStorageService>.Service; }
		}

		protected IXmlService XmlService
		{
			get { return LazyResolver<IXmlService>.Service; }
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

		public HomeViewModel()
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
			NavigationService.Navigate(Views.ADMIN_COLLECTION_HOME);
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
				//TODO : if no help in language show english help
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
				MessageDialogService.Show(Dialogs.ADMIN_MAILERROR);
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