#region Usings

using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin
{
	public class InstallVoiceSynthesisViewModel : AbstractViewModel
	{
		public IInstallVoiceSynthesisService InstallVoiceSynthesisService
		{
			get { return LazyResolver<IInstallVoiceSynthesisService>.Service; }
		}

		#region Commands

		public ICommand InstallVoiceSynthesisEngineCommand { get; private set; }
		public ICommand InstallLanguagePackCommand { get; private set; }
		public ICommand EnableVoiceSynthesisEngineCommand { get; private set; }

		#endregion


		public InstallVoiceSynthesisViewModel()
		{
			InstallVoiceSynthesisEngineCommand = new DelegateCommand(InstallVoiceSynthesisEngineAction);
			InstallLanguagePackCommand = new DelegateCommand(InstallLanguagePackAction);
			EnableVoiceSynthesisEngineCommand = new DelegateCommand(EnableVoiceSynthesisEngineAction);
		}

		#region Commands implementation

		private void EnableVoiceSynthesisEngineAction()
		{
			InstallVoiceSynthesisService.EnableVoiceSynthesisEngine();
		}

		private void InstallLanguagePackAction()
		{
			InstallVoiceSynthesisService.InstallLanguagePack();
		}

		private void InstallVoiceSynthesisEngineAction()
		{
			InstallVoiceSynthesisService.InstallVoiceSynthesisEngine();
		}

		#endregion

	}
}