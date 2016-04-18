using Storm.Mvvm;

namespace IndiaRose.Data.UIModel
{
    /// <summary>
    /// Classe contenant une couleur sous forme de string
    /// </summary>
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
