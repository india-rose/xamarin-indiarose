
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    public interface IMediaService
    {
        Task<string> GetPictureFromCameraAsync();

	    Task<string> GetPictureFromGalleryAsync();

        void RecordSound();
        string StopRecord();
    }
}
