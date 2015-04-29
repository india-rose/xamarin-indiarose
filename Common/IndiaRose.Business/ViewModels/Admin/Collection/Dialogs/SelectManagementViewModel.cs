using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
	public class SelectManagementViewModel : AbstractSettingsViewModel
	{
		private IndiagramContainer _AddIndiagram;
		private IndiagramContainer _currentIndiagram;

		[NavigationParameter]
		public IndiagramContainer CurrentIndiagram
		{
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}

		[NavigationParameter]
		public IndiagramContainer AddIndiagram
		{
			get { return _AddIndiagram; }
			set { SetProperty(ref _AddIndiagram, value); }
		}
		public ICommand SendIndiagramCommand{ get; private set; }

		public SelectManagementViewModel()
        {
			SendIndiagramCommand = new DelegateCommand(SendIndiagramAction);
        }
        private void SendIndiagramAction()
        {
	        _AddIndiagram.Indiagram.Parent = new Category(_currentIndiagram.Indiagram);
			BackAction();
        }
	}
}
