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
        public int Width => (int)Window.Current.Bounds.Width;

        public int Height => (int)Window.Current.Bounds.Height;
    }
}
