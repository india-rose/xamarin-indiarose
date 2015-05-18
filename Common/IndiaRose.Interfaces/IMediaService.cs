
using System;
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    public interface IMediaService
    {
        Task<string> GetPictureFromCameraAsync();

        Task<string> GetPictureFromGalleryAsync(); 
        Task<string> GetSoundFromGalleryAsync();

        void RecordSound();
        string StopRecord();
        void PlaySound(string url, Action callbackAction=null);
    }
}
