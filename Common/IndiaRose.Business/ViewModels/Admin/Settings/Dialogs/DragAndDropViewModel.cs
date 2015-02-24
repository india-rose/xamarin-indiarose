#region Usings

using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	
	public class DragAndDropViewModel : ViewModelBase
	{
		#region Fields

		protected ISettingsService SettingsService;

		#endregion

		#region Properties backing fields

		private bool _isDragAndDropEnabled;

		#endregion

		#region Public properties

		public bool IsDragAndDropEnabled
		{
			get { return _isDragAndDropEnabled; }
			set { SetProperty(ref _isDragAndDropEnabled, value); }
		}

		#endregion

		#region Commands

		public ICommand OkCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		#endregion

		#region Constructor

		public DragAndDropViewModel(IContainer container) : base(container)
		{
			OkCommand = new DelegateCommand(OkAction);
			CancelCommand = new DelegateCommand(CancelAction);

		    SettingsService = container.Resolve<ISettingsService>();
			_isDragAndDropEnabled = SettingsService.IsDragAndDropEnabled;
		}

		#endregion

		#region Commands implementation

		private void OkAction()
		{
		    SettingsService.IsDragAndDropEnabled = _isDragAndDropEnabled;
            SettingsService.Save();
		}

		private void CancelAction()
		{
			//Just close the dialog
		}

		#endregion
	}
}