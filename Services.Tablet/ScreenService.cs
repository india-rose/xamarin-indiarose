using Windows.UI.Xaml;
using IndiaRose.Interfaces;

namespace Services.Tablet
{
    public class ScreenService : IScreenService
    {
        public int Width
        {

			get { return (int)Window.Current.Bounds.Width; }
        }

        public int Height
        {
			get { return (int)Window.Current.Bounds.Height; }
        }
    }
}
