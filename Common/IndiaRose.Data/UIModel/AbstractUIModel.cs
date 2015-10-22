using Storm.Mvvm;

namespace IndiaRose.Data.UIModel
{
	public abstract class AbstractUIModel<T> : NotifierBase
	{
		private T _model;

		public T Model
		{
			get { return _model; }
			set { SetProperty(ref _model, value); }
		}

		protected AbstractUIModel()
		{
			
		}

		protected AbstractUIModel(T model)
		{
			Model = model;
		}
	}
}
