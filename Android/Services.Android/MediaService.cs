using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using IndiaRose.Framework;
using IndiaRose.Interfaces;
using Java.IO;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using Path = System.IO.Path;
using Uri = Android.Net.Uri;

namespace IndiaRose.Services.Android
{
    public class MediaService : AbstractAndroidService, IMediaService
    {
        private MediaRecorder _recorder;
        private String _url;

        public void RecordSound()
        {
            //todo utiliser le storage service
            _url = string.Format(Environment.ExternalStorageDirectory.Path + "/IndiaRose/sound/IndiaRose_sound_{0}.3gpp", Guid.NewGuid());
            _recorder = new MediaRecorder();
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            _recorder.SetOutputFile(_url);
            _recorder.Prepare();
            _recorder.Start();
        }

        public string StopRecord()
        {
            _recorder.Stop();
            return _url;
        }

        public Task<string> GetPictureFromCameraAsync()
        {
            return AsyncHelper.CreateAsyncFromCallback<string>(callbackResult =>
            {
                string path = Path.Combine(Environment.ExternalStorageDirectory.Path, string.Format("IndiaRose/image/IndiaRose_photo_{0}.jpg", Guid.NewGuid()));
                File file = new File(path);

                Intent intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(file));
                ActivityService.StartActivityForResult(intent, (result, data) =>
                {
                    if (result == Result.Ok)
                    {
                        callbackResult(path);
                    }
                    else
                    {
                        callbackResult(null);
                    }
                });
            });
        }
        private string SavePhoto(Bitmap bitmap)
        {
            string filename = Path.Combine(Environment.ExternalStorageDirectory.Path, string.Format("IndiaRose/image/IndiaRose_photo_{0}.png", Guid.NewGuid()));

            using (System.IO.Stream stream = System.IO.File.OpenWrite(filename))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
            }

            return filename;
        }
        public Task<string> GetPictureFromGalleryAsync()
        {
            return AsyncHelper.CreateAsyncFromCallback<string>(resultCallback =>
            {

                Intent intent = new Intent();
                // Show only images, no videos or anything else
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);
                // Always show the chooser (if there are multiple options available)
                var trad = DependencyService.Container.Resolve<ILocalizationService>();
                ActivityService.StartActivityForResult(Intent.CreateChooser(intent, trad.GetString("PictureSelection","Text")),
                    (result, data) =>
                    {
                        string path = null;
                        if (result == Result.Ok)
                        {
                            Uri selectedImage = data.Data;

                            ParcelFileDescriptor parcelFileDescriptor = ActivityService.CurrentActivity.ContentResolver.OpenFileDescriptor(selectedImage, "r");
                            FileDescriptor fileDescriptor = parcelFileDescriptor.FileDescriptor;
                            Bitmap photo = BitmapFactory.DecodeFileDescriptor(fileDescriptor);
                            parcelFileDescriptor.Close();
                            path = SavePhoto(photo);
                        }
                        resultCallback(path);
                    });
            });
        }
        public Task<string> GetSoundFromGalleryAsync()
        {
            return AsyncHelper.CreateAsyncFromCallback<string>(resultCallback =>
            {
                Intent intent = new Intent();
                intent.SetType("audio/*");
                intent.SetAction(Intent.ActionGetContent); 
                var trad = DependencyService.Container.Resolve<ILocalizationService>();
                ActivityService.StartActivityForResult(Intent.CreateChooser(intent, trad.GetString("SoundSelection","Text")), (result, data) =>
                {
                    string path = null;
                    if (result == Result.Ok)
                    {
                        Uri selectedSound = data.Data;
                        ParcelFileDescriptor parcelFileDescriptor = ActivityService.CurrentActivity.ContentResolver.OpenFileDescriptor(selectedSound, "r");
                        FileDescriptor fileDescriptor = parcelFileDescriptor.FileDescriptor;
                        FileInputStream inputStream = new FileInputStream(fileDescriptor); 
                        path = Path.Combine(Environment.ExternalStorageDirectory.Path,
                               string.Format("IndiaRose/sound/IndiaRose_sound_{0}.3gp", Guid.NewGuid()));
                        File outputFile = new File(path);
                        InputStream inStream = null;
                        OutputStream outStream = null;
                        inStream = inputStream;
                        outStream = new FileOutputStream(outputFile);

                        byte[] buffer = new byte[1024];

                        int length;
                        //copy the file content in bytes 
                        while ((length = inStream.Read(buffer)) > 0)
                        {
                            outStream.Write(buffer, 0, length);
                        }
                        inStream.Close();
                        outStream.Close();
                    }
                    resultCallback(path);
                });
            });
        }
    }
}