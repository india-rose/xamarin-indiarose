#region Usings

using System.Windows.Input;
using Storm.Mvvm.Commands;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
    /// <summary>
    /// VueModèle de la page de menu des préférences
    /// </summary>
	public class SettingsListViewModel : AbstractViewModel
	{
		#region Commands

		public ICommand ApplicationLookCommand { get; private set; }
		public ICommand IndiagramPropertyCommand { get; private set; }
		public ICommand DragAndDropCommand { get; private set; }

		public ICommand ReadingDelayCommand { get; private set; }
		public ICommand CategoryReadingCommand { get; private set; }
		public ICommand ResetSettingsCommand { get; private set; }

		#endregion

		public SettingsListViewModel()
		{
			ApplicationLookCommand = new DelegateCommand(ApplicationLookAction);
			ReadingDelayCommand = new DelegateCommand(ReadingDelayAction);
			DragAndDropCommand = new DelegateCommand(DragAndDropAction);
			CategoryReadingCommand = new DelegateCommand(CategoryReadingAction);
            ResetSettingsCommand = new DelegateCommand(ResetSettingsAction);
			IndiagramPropertyCommand = new DelegateCommand(IndiagramPropertyAction);
		}

		#region Commands implementation

        /// <summary>
        /// Ouvre la page de réglages des couleurs de fond de l'application et taille des zones de la partie utilisateur
        /// </summary>
		private void ApplicationLookAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_APPLICATIONLOOK);
		}

        /// <summary>
        /// Ouvre la page de réglages des propriétés des Indiagrams
        /// </summary>
		private void IndiagramPropertyAction()
		{
			NavigationService.Navigate(Views.ADMIN_SETTINGS_INDIAGRAMPROPERTIES);
		}

        /// <summary>
        /// Ouvre le dialogue de réglages du drag and drop
        /// </summary>
		private void DragAndDropAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_DRAGANDDROP);
		}

        /// <summary>
        /// Ouvre le dialogue de réglages de la lecture des catégories
        /// </summary>
		private void CategoryReadingAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_CATEGORYREADING);
		}

        /// <summary>
        /// Ouvre le dialogue de réglages du délai de lecture
        /// </summary>
		private void ReadingDelayAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_READINGDELAY);
		}

        /// <summary>
        /// Ouvre le dialogue de réinitilisation des réglages
        /// </summary>
		private void ResetSettingsAction()
		{
			MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_RESETSETTINGS);
		}

		#endregion
	}
}