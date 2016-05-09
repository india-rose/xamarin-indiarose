using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Interfaces;
using SharpDX.XAudio2;
using Storm.Mvvm.Inject;

namespace Services.Tablet
{

    public class MediaService : IMediaService
    {
        private MediaCapture _recordMediaCapture;
        private StorageFile _recordStorageFile;
        private String _url;
        private MediaElement _sound;

        public IStorageService StorageService
        {
            get { return LazyResolver<IStorageService>.Service; }
        }

        public async Task<string> GetPictureFromCameraAsync()
        {
            var photo = new CameraCaptureUI();

            photo.PhotoSettings.AllowCropping = true;
            photo.PhotoSettings.CroppedAspectRatio = new Size(1, 1);
            photo.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            photo.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;

            _recordStorageFile = await photo.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (_recordStorageFile != null)
            {
                var path = Path.Combine(StorageService.ImagePath);
                _url = string.Format("Image_{0}.{1}", Guid.NewGuid(), _recordStorageFile.FileType);
                var folder = await StorageFolder.GetFolderFromPathAsync(path);

                await _recordStorageFile.MoveAsync(folder, _url, NameCollisionOption.FailIfExists);

                return string.Format("{0}\\{1}",path, _url);
            }

            return "";
        }

        public async Task<string> GetPictureFromGalleryAsync()
		{
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);

                var path = Path.Combine(StorageService.ImagePath);
                _url = String.Format("Image_{0}.{1}", Guid.NewGuid(), file.FileType);
                var folder = await StorageFolder.GetFolderFromPathAsync(path);

                //TODO rajouter le code pour le redimensionnement de l'image

                await file.CopyAsync(folder, _url);
                return string.Format("{0}\\{1}", path, _url);
            }

            return "";
		}

        public async Task<string> GetSoundFromGalleryAsync()
        {
            //Create a new picker
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.MusicLibrary
            };

            //Add filetype filters.
            filePicker.FileTypeFilter.Add(".mp3");  // Ok
            filePicker.FileTypeFilter.Add(".m4a");  // Ok
            filePicker.FileTypeFilter.Add(".3gp");  // Ok
            filePicker.FileTypeFilter.Add(".3gpp"); // Ok
            filePicker.FileTypeFilter.Add(".wma");  // Ok
            filePicker.FileTypeFilter.Add(".flac"); // Ok
            filePicker.FileTypeFilter.Add(".wav");  // Ok
            filePicker.FileTypeFilter.Add(".mp4");  // Ok, même en donnant un fichier vidéo :D
            /*
            filePicker.FileTypeFilter.Add(".ogg");  // Pas testé
            filePicker.FileTypeFilter.Add(".oga");  // Pas testé
            filePicker.FileTypeFilter.Add(".alac"); // Pas testé
            filePicker.FileTypeFilter.Add(".m4p");  // Pas testé
            filePicker.FileTypeFilter.Add(".m4b");  // Pas testé
            filePicker.FileTypeFilter.Add(".aac");  // Pas testé
            */


            //Retrieve file from picker
            StorageFile file = await filePicker.PickSingleFileAsync();

            if (file != null)
            {
                var path = Path.Combine(StorageService.SoundPath);
                var folder = await StorageFolder.GetFolderFromPathAsync(path);
                _url = string.Format("Sound_{0}.{1}", Guid.NewGuid(), file.FileType);
                await file.CopyAsync(folder, _url);

                return string.Format("{0}\\{1}", path, _url);
            }

            return "";
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

            _url = string.Format("Sound_{0}.{1}", Guid.NewGuid(), "aac");
            var path = Path.Combine(StorageService.SoundPath);
            var folder = await StorageFolder.GetFolderFromPathAsync(path);

            _recordStorageFile = await folder.CreateFileAsync(_url);
            MediaEncodingProfile profil = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto);
            await _recordMediaCapture.StartRecordToStorageFileAsync(profil, _recordStorageFile);
        }

        public async Task<string> StopRecord()
        {
            await _recordMediaCapture.StopRecordAsync();
            return Path.Combine(StorageService.SoundPath,_url);
        }
    }
}
