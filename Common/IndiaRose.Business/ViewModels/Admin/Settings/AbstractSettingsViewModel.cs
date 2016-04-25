using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
    /// <summary>
    /// VueModèle abstrait dont les pages de configuration doivent hériter
    /// </summary>
	public abstract class AbstractSettingsViewModel : AbstractViewModel
	{
		public ISettingsService SettingsService => LazyResolver<ISettingsService>.Service;

        public ICommand SaveCommand { get; private set; }

		protected AbstractSettingsViewModel()
		{
			SaveCommand = new DelegateCommand(SaveAction);
		}

        /// <summary>
        /// Action de sauvegarde des préférences
        /// Utilise le SettingsService
        /// </summary>
		protected virtual void SaveAction()
		{
		    SettingsService.SaveAsync();
            CloseDialogAction();
		}
	}
}
