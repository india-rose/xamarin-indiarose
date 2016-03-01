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
        //TODO doc
	    event EventHandler CollectionImported;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipStream"></param>
        /// <returns></returns>
        /// <seealso cref="IResourceService.OpenZip"/>
	    Task InitializeCollectionFromZipStreamAsync(Stream zipStream);

        /// <summary>
        /// Vérifie de manière si l'ancien format de stockage de la collection est présent
        /// </summary>
        /// <returns>La tâche asynchrone vérifiant la présence de l'ancien format de sauvegarde</returns>
	    Task<bool> HasOldCollectionFormatAsync();

        /// <summary>
        /// Charge de manière asynchrone la collection avec l'ancien format de stockage
        /// </summary>
        /// <returns>La tâche asynchrone chargeant la collection</returns>
	    Task InitializeCollectionFromOldFormatAsync();
    }
}
