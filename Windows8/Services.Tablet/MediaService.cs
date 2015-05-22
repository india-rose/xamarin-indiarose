using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Services
{
    public class MediaService : IMediaService
    {
        private MediaCapture _recordMediaCapture;
        private StorageFile _recordStorageFile;
        private String _url;

        public IStorageService StorageService
        {
            get { return LazyResolver<IStorageService>.Service; }
        }

        public async Task<string> GetPictureFromCameraAsync()
        {
            var photo = new CameraCaptureUI();
            var url = StorageService.GenerateFilename(StorageType.Image, "png");

            photo.PhotoSettings.AllowCropping = true;
            photo.PhotoSettings.CroppedAspectRatio = new Size(1, 1);
            photo.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;

            _recordStorageFile = await photo.CaptureFileAsync(CameraCaptureUIMode.Photo);

            //TODO vérifier l'emplacement de la capture
            _recordStorageFile.MoveAsync(ApplicationData.Current.LocalFolder, url);

            return url;
        }

        public async Task<string> GetPictureFromGalleryAsync()
		{
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                Image imageChangedProfilePic = new Image {Source = image, Stretch = Stretch.Fill};
                return null;
            }
            else
            {
                //  OutputTextBlock.Text = "Operation cancelled.";
                return null;
            }
        }

        public Task<string> GetSoundFromGalleryAsync()
		{
			return null;
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
