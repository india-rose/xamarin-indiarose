using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
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
