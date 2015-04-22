#region Usings

using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;

#endregion

namespace IndiaRose.Business.ViewModels
{
	public abstract class AbstractViewModel : ViewModelBase
	{
		public ICommand BackCommand { get; private set; }

		protected AbstractViewModel()
		{
			BackCommand = new DelegateCommand(BackAction);
		}

		protected virtual void BackAction()
		{
			NavigationService.GoBack();
		}
	}
}