#region Usings

using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class CategoryReadingViewModel : ViewModelBase
	{
		#region Fields

	    private bool _isCategoryReadingEnabled;

		protected ISettingsService SettingsService;

		#endregion

		#region Public properties

		public bool IsCategoryReadingEnabled
		{
			get { return _isCategoryReadingEnabled; }
			set { SetProperty(ref _isCategoryReadingEnabled, value); }
		}

		#endregion

		#region Commands

		public ICommand OkCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		#endregion
		
		#region Constructor

		public CategoryReadingViewModel(IContainer container) : base(container)
		{
			OkCommand = new DelegateCommand(OkAction);
			CancelCommand = new DelegateCommand(CancelAction);

			SettingsService = container.Resolve<ISettingsService>();
			IsCategoryReadingEnabled = SettingsService.IsCategoryNameReadingEnabled;
		}

		#endregion

		#region Commands implementation

		private void OkAction()
		{
			SettingsService.IsCategoryNameReadingEnabled = IsCategoryReadingEnabled;
			SettingsService.Save();
		}

		private void CancelAction()
		{

		}

		#endregion
	}
}
