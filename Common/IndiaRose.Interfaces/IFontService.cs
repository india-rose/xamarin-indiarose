using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    public interface IFontService
    {
        Dictionary<string, string> FontList { get; }
    }
}
