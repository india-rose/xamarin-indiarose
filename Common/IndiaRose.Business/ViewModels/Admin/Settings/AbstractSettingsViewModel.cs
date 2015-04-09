using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
	public abstract class AbstractSettingsViewModel : AbstractViewModel
	{
		private ISettingsService _settingsService;

		public ISettingsService SettingsService
		{
			get { return _settingsService ?? (_settingsService = Container.Resolve<ISettingsService>()); }
		}

		public ICommand SaveCommand { get; private set; }

		protected AbstractSettingsViewModel(IContainer container) : base(container)
		{
			SaveCommand = new DelegateCommand(SaveAction);
		}

		protected virtual void SaveAction()
		{
			SettingsService.SaveAsync();
		}
	}
}
