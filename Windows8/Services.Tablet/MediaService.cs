using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Tablet
{
    class MediaService : AbstractWindowsService, IMediaService
    {
        public IStorageService StorageService
        {
            get { return LazyResolver<IStorageService>.Service; }
        }
        //private MediaRecorder _recorder;
        private string _url;

        public Task<string> GetPictureFromCameraAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPictureFromGalleryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSoundFromGalleryAsync()
        {
            throw new NotImplementedException();
        }

        public void RecordSound()
        {
            throw new NotImplementedException();
        }

        public string StopRecord()
        {
            throw new NotImplementedException();
        }

        public void PlaySound(string url)
        {
            throw new NotImplementedException();
        }
    }
}
