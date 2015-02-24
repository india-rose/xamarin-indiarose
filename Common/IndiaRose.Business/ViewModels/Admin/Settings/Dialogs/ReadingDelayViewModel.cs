using System;
using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class ReadingDelayViewModel : ViewModelBase
	{
		#region Fields

		private readonly ISettingsService _settingsService;

		#endregion

		#region Properties backing fields

		private int _currentValue = 10;
		private float _currentDelay = 1.0f;

		#endregion

		#region Public properties

		public int CurrentValue
		{
			get { return _currentValue; }
			set
			{
				if (SetProperty(ref _currentValue, value))
				{
					CurrentDelay = value/10.0f;
				}
			}
		}

		public float CurrentDelay
		{
			get { return _currentDelay; }
			set { SetProperty(ref _currentDelay, value); }
		}

		#endregion

		#region Commands

		public ICommand OkCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		#endregion

		#region Constructor

		public ReadingDelayViewModel(IContainer container)
			: base(container)
		{
			OkCommand = new DelegateCommand(OkAction);
			CancelCommand = new DelegateCommand(CancelAction);

			_settingsService = container.Resolve<ISettingsService>();
			// le round est là pour les problèmes de stockage de flottants et des arrondis qui en résultent
			CurrentValue = (int) Math.Round(_settingsService.TimeOfSilenceBetweenWords * 10.0);
		}

		#endregion

		#region Commands implementation

		private void OkAction()
		{
			_settingsService.TimeOfSilenceBetweenWords = CurrentDelay;
            _settingsService.Save();
		}

		private void CancelAction()
		{

		}

		#endregion
	}
}
