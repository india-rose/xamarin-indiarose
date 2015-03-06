using Storm.Mvvm;

namespace IndiaRose.Data.UIModel
{
	public class ColorContainer : NotifierBase
	{
		private string _color;

		public string Color
		{
			get { return _color; }
			set { SetProperty(ref _color, value); }
		}
	}
}
