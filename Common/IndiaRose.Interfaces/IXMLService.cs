using System;
using System.IO;
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit les interfaces pour la gestion d'un XML de sauvegarde (ancienne version de sauvegarde)
    /// Implémentation cross-plateforme
    /// </summary>
    public interface IXmlService
    {
        //TODO
	    event EventHandler CollectionImported;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipStream"></param>
        /// <returns></returns>
        /// <seealso cref="IResourceService.OpenZip"/>
	    Task InitializeCollectionFromZipStreamAsync(Stream zipStream);

	    Task<bool> HasOldCollectionFormatAsync();

	    Task InitializeCollectionFromOldFormatAsync();
    }
}
