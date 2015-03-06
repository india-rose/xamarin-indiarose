using System;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class ReadingDelayViewModel : AbstractSettingsViewModel
	{
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

		#region Constructor

		public ReadingDelayViewModel(IContainer container)
			: base(container)
		{
			// Math.Round is needed because of float precision issue
			CurrentValue = (int) Math.Round(SettingsService.TimeOfSilenceBetweenWords * 10.0);
		}

		#endregion

		#region Commands implementation

		protected override void SaveAction()
		{
			SettingsService.TimeOfSilenceBetweenWords = CurrentDelay;
			base.SaveAction();
		}

		#endregion
	}
}
