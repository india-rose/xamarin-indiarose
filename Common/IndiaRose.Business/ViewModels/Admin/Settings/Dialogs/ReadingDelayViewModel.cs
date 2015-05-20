using System;

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class ReadingDelayViewModel : AbstractSettingsViewModel
	{
		#region Properties backing fields

		private int _currentValue = 10;
		private float _currentDelay = 1.0f;
	    private string _currentValueStr="1.0 secondes";

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
                    CurrentValueStr = CurrentDelay + "secondes";
				}
			}
		}

	    public String CurrentValueStr
	    {
            get { return _currentValueStr; }
	        set
	        {
	            SetProperty(ref _currentValueStr, value);
	        }
	    }

		public float CurrentDelay
		{
			get { return _currentDelay; }
		    set
		    {
		        SetProperty(ref _currentDelay, value);
                CurrentValueStr = CurrentDelay + "secondes";
		    }
		}

		#endregion

		#region Constructor

		public ReadingDelayViewModel()
		{
			// Math.Round is needed because of float precision issue
			CurrentValue = (int) Math.Round(SettingsService.TimeOfSilenceBetweenWords * 10.0);
		    CurrentValueStr = _currentValueStr;
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
