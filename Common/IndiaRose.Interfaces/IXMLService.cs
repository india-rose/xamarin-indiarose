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
        /// <summary>
        /// Événement indiquant la fin du chargement de la collection
        /// Levé par InitializeCollectionFromOldFormatAsync ou InitializeCollectionFromZipStreamAsync
        /// </summary>
        /// <see cref="InitializeCollectionFromZipStreamAsync"/>
        /// <seealso cref="InitializeCollectionFromOldFormatAsync"/>
	    event EventHandler CollectionImported;

        /// <summary>
        /// Charge la collection des Indiagrams à partir de l'archive de la collection
        /// Lève CollectionImported à la fin du chargement
        /// Méthode asynchrone
        /// </summary>
        /// <param name="zipStream">Le Stream de l'archive de la Collection</param>
        /// <returns>La tâche asynchrone chargeant la collection</returns>
        /// <seealso cref="IResourceService.OpenZip"/>
	    Task InitializeCollectionFromZipStreamAsync(Stream zipStream);

        /// <summary>
        /// Vérifie de manière si l'ancien format de stockage de la collection est présent
        /// Méthode asynchrone
        /// </summary>
        /// <returns>La tâche asynchrone vérifiant la présence de l'ancien format de sauvegarde</returns>
	    Task<bool> HasOldCollectionFormatAsync();

        /// <summary>
        /// Charge de manière asynchrone la collection avec l'ancien format de stockage
        /// Lève CollectionImported à la fin du chargement
        /// Méthode asynchrone
        /// </summary>
        /// <returns>La tâche asynchrone chargeant la collection</returns>
	    Task InitializeCollectionFromOldFormatAsync();

        /// <summary>
        /// Vérifie de manière asynchrone si l'ancien format de stockage des paramétres est présent
        /// </summary>
        /// <returns>La tâche asynchrone vérifiant la présence des anciens paramétres</returns>
        Task<bool> HasOldSettingsAsync();

        /// <summary>
        /// Charge de manière asynchrone les paramétres avec l'ancien format de stockage
        /// </summary>
        /// <returns>La tâche asynchrone chargeant les paramétres</returns>
        Task ReadOldSettings();
    }
}
