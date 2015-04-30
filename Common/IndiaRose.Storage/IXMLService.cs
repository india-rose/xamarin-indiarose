using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Storage
{
    public interface IXmlService
    {
        void Initialize(Stream path);
    }
}
