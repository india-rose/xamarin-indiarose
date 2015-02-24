#region Usings

using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels
{
	public abstract class AbstractBackViewModel : ViewModelBase
	{
		public ICommand BackCommand { get; private set; }

		protected AbstractBackViewModel(IContainer container) : base(container)
		{
			BackCommand = new DelegateCommand(BackAction);
		}

		protected void BackAction()
		{
			NavigationService.GoBack();
		}
	}
}