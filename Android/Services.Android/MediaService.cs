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
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace IndiaRose.Services.Android
{
    public class MediaService : AbstractAndroidService, IMediaService
    {
        public IStorageService StorageService
        {
            get { return LazyResolver<IStorageService>.Service; }
        }
        private MediaRecorder _recorder;
        private string _url;

        public void RecordSound()
        {
            _url = StorageService.GenerateFilename(StorageType.Sound, "3gpp");
            _recorder = new MediaRecorder();
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            _recorder.SetOutputFile(_url);
            _recorder.Prepare();
            _recorder.Start();
        }

        public Task<string> StopRecord()
        {
            return AsyncHelper.CreateAsyncFromCallback<string>(callbackResult =>
            {
                _recorder.Stop();
                callbackResult(_url);
            });
        }

        public Task<string> GetPictureFromCameraAsync()
        {
            return AsyncHelper.CreateAsyncFromCallback<string>(callbackResult =>
            {
                string path = StorageService.GenerateFilename(StorageType.Image, "png");
                File file = new File(path);

                Intent intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(file));
                ActivityService.StartActivityForResult(intent, (result, data) =>
                {
                    if (result == Result.Ok)
                    {
                        PerformCrop(Uri.FromFile(file), callbackResult, path);
                    }
                    else
                    {
                        callbackResult(null);
                    }
                });
            });
        }

        /// <summary>
        /// Lance la redimensionnement d'une image
        /// </summary>
        /// <param name="picUri">Uri de l'image � redimensionner</param>
        /// <param name="callbackResult">Callback � executer lorsque le redimensionnement est termin�</param>
        /// <param name="path">Chemin de l'acc�s � l'image redimensionn�</param>
        private void PerformCrop(Uri picUri, Action<string> callbackResult, string path)
        {
            Intent cropIntent = new Intent("com.android.camera.action.CROP");
            // indicate image type and Uri
            cropIntent.SetDataAndType(picUri, "image/*");
            // set crop properties
            cropIntent.PutExtra("crop", "true");
            // indicate aspect of desired crop
            cropIntent.PutExtra("aspectX", 1);
            cropIntent.PutExtra("aspectY", 1);
            // retrieve data on return
            cropIntent.PutExtra(MediaStore.ExtraOutput, picUri);
            // start the activity - we handle returning in onActivityResult
            ActivityService.StartActivityForResult(cropIntent, (result, data) =>
            {
				callbackResult(result == Result.Ok ? path : null);
            });
        }

        /// <summary>
        /// Sauvegarde une image bitmap sous forme de png
        /// </summary>
        /// <param name="bitmap">L'image bitmap � enregistrer</param>
        /// <returns>Le chemin d'acc�s � l'image</returns>
        private string SavePhoto(Bitmap bitmap)
        {
            string filename = StorageService.GenerateFilename(StorageType.Image, "png");
	        try
	        {
		        using (System.IO.Stream stream = System.IO.File.OpenWrite(filename))
		        {
			        bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
		        }
	        }
	        catch (Exception)
	        {
		        //TODO : log error
		        return null;
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
                ActivityService.StartActivityForResult(Intent.CreateChooser(intent, trad.GetString("ImageChoice_PickerTitle", "Text")),
                    (result, data) =>
                    {
                        if (result == Result.Ok)
                        {
                            Uri selectedImage = data.Data;

                            ParcelFileDescriptor parcelFileDescriptor = ActivityService.CurrentActivity.ContentResolver.OpenFileDescriptor(selectedImage, "r");
                            FileDescriptor fileDescriptor = parcelFileDescriptor.FileDescriptor;
	                        try
	                        {
		                        Bitmap photo = BitmapFactory.DecodeFileDescriptor(fileDescriptor);
		                        parcelFileDescriptor.Close();

		                        if (photo == null)
		                        {
			                        //TODO : log error
			                        resultCallback(null);
			                        return;
		                        }

		                        string path = SavePhoto(photo);

		                        if (path == null)
		                        {
									//TODO : log error
			                        resultCallback(null);
			                        return;
		                        }

		                        PerformCrop(Uri.FromFile(new File(path)), resultCallback, path);
	                        }
	                        catch (Exception)
	                        {
		                        //TODO : log error
								resultCallback(null);
	                        }
                        }
                        else
                        {
                            resultCallback(null);
                        }
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
                ActivityService.StartActivityForResult(Intent.CreateChooser(intent, trad.GetString("SoundChoice_PickerTitle", "Text")), (result, data) =>
                {
                    string path = null;
                    if (result == Result.Ok)
                    {
                        Uri selectedSound = data.Data;
                        ParcelFileDescriptor parcelFileDescriptor = ActivityService.CurrentActivity.ContentResolver.OpenFileDescriptor(selectedSound, "r");
                        FileDescriptor fileDescriptor = parcelFileDescriptor.FileDescriptor;
                        FileInputStream inputStream = new FileInputStream(fileDescriptor);
                        path = StorageService.GenerateFilename(StorageType.Sound, "3gpp");
                        File outputFile = new File(path);
	                    InputStream inStream = inputStream;
                        OutputStream outStream = new FileOutputStream(outputFile);

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