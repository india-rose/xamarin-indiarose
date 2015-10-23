using System;
using IndiaRose.Data.Model;
using Storm.Mvvm.Events;

namespace IndiaRose.Data.UIModel
{
	public class IndiagramUIModel : AbstractUIModel<Indiagram>
	{
		public event EventHandler ReinforcerStateChanged;

		private bool _isReinforcerEnabled;

		public bool IsReinforcerEnabled
		{
			get { return _isReinforcerEnabled; }
			set
			{
				if (SetProperty(ref _isReinforcerEnabled, value))
				{
					this.RaiseEvent(ReinforcerStateChanged);
				}
			}
		}

		public IndiagramUIModel()
		{
			
		}

		public IndiagramUIModel(Indiagram model) : base(model)
		{
			
		}
	}
}
