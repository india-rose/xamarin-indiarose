using System;
using System.IO;
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    public interface IXmlService
    {
	    event EventHandler CollectionImported;

	    Task InitializeCollectionFromZipStreamAsync(Stream zipStream);

	    Task<bool> HasOldCollectionFormatAsync();

	    Task InitializeCollectionFromOldFormatAsync();
    }
}
