using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Provider;
using IndiaRose.Framework;
using IndiaRose.Interfaces;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace IndiaRose.Services.Android
{
    public class MediaService : AbstractAndroidService,IMediaService
    {
        private MediaRecorder _recorder;
        private String _url;

	    public void RecordSound()
        {
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

        public Task<string> GetPictureFromGalleryAsync()
        {
            return AsyncHelper.CreateAsyncFromCallback<string>(resultCallback =>
            {

                Intent intent = new Intent();
                // Show only images, no videos or anything else
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);
                // Always show the chooser (if there are multiple options available)
                ActivityService.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"),
                    (result, data) =>
                    {
                        string path = null;
                        if (result == Result.Ok)
                        {
                            Uri selectedImage = data.Data;
                            string res = selectedImage.Path;
                            path = Path.Combine(Environment.ExternalStorageDirectory.Path, string.Format("IndiaRose/image/IndiaRose_photo_{0}.jpg", Guid.NewGuid()));
                            System.IO.File.Copy(res, path);
                            //TODO le res va pas

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
                ActivityService.StartActivityForResult(Intent.CreateChooser(intent, "Select Sound"), (result, data) =>
                {
                    string path = null;
                    if (result == Result.Ok)
                    {
                        Uri selectedImage = data.Data;
                        string res = selectedImage.Path;
                        path = Path.Combine(Environment.ExternalStorageDirectory.Path, string.Format("IndiaRose/sound/IndiaRose_sound_{0}.jpg", Guid.NewGuid()));
                        System.IO.File.Copy(res, path);
                        //TODO le res va pas
                    }
                    resultCallback(path);
                });
            });
        }
    }
}