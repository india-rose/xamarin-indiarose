#region Usings

using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels
{
	public abstract class AbstractViewModel : ViewModelBase
	{
		public ICommand BackCommand { get; private set; }

		protected AbstractViewModel(IContainer container) : base(container)
		{
			BackCommand = new DelegateCommand(BackAction);
		}

		protected virtual void BackAction()
		{
			NavigationService.GoBack();
		}
	}
}