
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    public interface IMediaService
    {
        Task<string> GetPictureFromCameraAsync();

        Task<string> GetPictureFromGalleryAsync(); 
        Task<string> GetSoundFromGalleryAsync();

        void RecordSound();
        Task<string> StopRecord();
        void PlaySound(string url);
    }
}
