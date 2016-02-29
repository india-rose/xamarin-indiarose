using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Interface qui fournit les méthodes pour l'utilisation des média (Son & Caméra)
    /// L'implémentation dépend de la plateforme
    /// </summary>
    public interface IMediaService
    {
        //TODO doc
        Task<string> GetPictureFromCameraAsync();

        Task<string> GetPictureFromGalleryAsync(); 
        Task<string> GetSoundFromGalleryAsync();

        void RecordSound();
        Task<string> StopRecord();
    }
}