#region Usings

using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin
{
	public class InstallTextToSpeechViewModel : AbstractBackViewModel
	{
		public ICommand InstallIvonaCommand { get; private set; }
		public ICommand InstallPackCommand { get; private set; }
		public ICommand ChooseIvonaCommand { get; private set; }


		public InstallTextToSpeechViewModel(IContainer container) : base(container)
		{
			InstallIvonaCommand = new DelegateCommand(InstallIvonaAction);
			InstallPackCommand = new DelegateCommand(InstallPackAction);
			ChooseIvonaCommand = new DelegateCommand(ChooseIvonaAction);
		}

		private void ChooseIvonaAction()
		{
			Container.Resolve<IInstallTTSService>().chooseIvona();
		}

		private void InstallPackAction()
		{
			Container.Resolve<IInstallTTSService>().InstallPack();
		}

		private void InstallIvonaAction()
		{
			Container.Resolve<IInstallTTSService>().InstallIvona();
		}
	}
}