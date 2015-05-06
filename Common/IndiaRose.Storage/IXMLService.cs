using System.IO;

namespace IndiaRose.Storage
{
    public interface IXmlService
    {
	    void InitializeCollection(Stream zipStream);
    }
}
