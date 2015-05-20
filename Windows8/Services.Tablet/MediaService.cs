using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Tablet
{
    class MediaService : AbstractWindowsService, IMediaService
    {
        private MediaCapture _recordMediaCapture;
        private StorageFile _recordStorageFile;
        private String _url;

        public IStorageService StorageService
        {
            get { return LazyResolver<IStorageService>.Service; }
        }

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

        public async void RecordSound()
        {
            _recordMediaCapture = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Audio,
                MediaCategory = MediaCategory.Other,
                AudioProcessing = AudioProcessing.Default
            };
            await _recordMediaCapture.InitializeAsync(settings);
            _url = StorageService.GenerateFilename(StorageType.Sound, "aac");
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            _recordStorageFile = await localFolder.CreateFileAsync(_url);
            MediaEncodingProfile profil = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto);
            await _recordMediaCapture.StartRecordToStorageFileAsync(profil, _recordStorageFile);
        }

        public string StopRecord()
        {
            //TODO si ça plante, async
            _recordMediaCapture.StopRecordAsync();
            _recordMediaCapture.Dispose();
            return _url;
        }

        public void PlaySound(string url)
        {
            var path = new Uri(url, UriKind.Absolute);
            var play = new MediaElement
            {
                AutoPlay = false,
                Source = path
            };

            play.Play();
        }
    }
}
