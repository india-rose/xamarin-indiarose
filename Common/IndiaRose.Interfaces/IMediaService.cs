using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Interface qui fournit les méthodes pour l'utilisation des média (Son & Caméra)
    /// L'implémentation dépend de la plateforme
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// Utilise l'appareil photo du device pour prendre une photo, l'enregistre à la bonne taille dans le dossier Image et retourne son chemin d'accès
        /// Méthode asynchrone
        /// </summary>
        /// <returns>Le chemin d'accès de la photo prise</returns>
        /// <see cref="IStorageService"/>
        Task<string> GetPictureFromCameraAsync();

        /// <summary>
        /// Ouvre la galerie pour trouver une photo, l'enregistre dans le dossier Image et à la bonne et retourne son chemin d'accès
        /// Méthode asynchrone
        /// </summary>
        /// <returns>Le chemin d'accès de l'image</returns>
        /// <see cref="IStorageService"/>
        Task<string> GetPictureFromGalleryAsync(); 
        /// <summary>
        /// Ouvre la galerie pour trouver un son, l'enregistre dans le dossier Son et retourne son chemin d'accès
        /// Méthode asynchrone
        /// </summary>
        /// <returns>Le chemin d'accès du son</returns>
        /// <see cref="IStorageService"/>
        Task<string> GetSoundFromGalleryAsync();

        /// <summary>
        /// Lance l'enregistrement d'un son via le micro du device
        /// </summary>
        void RecordSound();
        /// <summary>
        /// Stop l'enregistrement et enregistre le son dans le dossier Son et retourne son chemin d'accès
        /// Méthode asynchrone
        /// </summary>
        /// <returns>Le chemin d'accès du son enregistré</returns>
        /// <see cref="IStorageService"/>
        Task<string> StopRecord();
    }
}