using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IndiaRose.Interfaces;
using IndiaRose.Services;

namespace IndiaRose.Tablet
{
    public abstract class AbstractWindowsService : AbstractService, IInitializable
    {
        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
