using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
    public class ScreenService : IScreenService
    {
        public int Width
        {
            
            get { return 1300; }
        }

        public int Height
        {
            get { return 760; }
        }
    }
}
