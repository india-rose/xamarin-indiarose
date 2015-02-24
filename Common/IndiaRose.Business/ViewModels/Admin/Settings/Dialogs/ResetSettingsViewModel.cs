#region Usings

using System;
using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion
namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    public class ResetSettingsViewModel : ViewModelBase
    {
        #region Fields

        private ISettingsService SettingsService = null;

        #endregion      

        #region Commands

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        #endregion

        #region Constructor
        public ResetSettingsViewModel(IContainer container) : base(container)
        {
			OkCommand = new DelegateCommand(OkAction);
			CancelCommand = new DelegateCommand(CancelAction);

	        SettingsService = container.Resolve<ISettingsService>();
        }

        #endregion

        #region Commands implementation

        private void OkAction()
        {
           SettingsService.Reset();
        }

        private void CancelAction()
        {
         

        }

        #endregion
    }
}
