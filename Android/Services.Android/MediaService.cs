using System;
using Android.Content;
using Android.Media;
using Android.Provider;
using IndiaRose.Interfaces;
using Java.IO;
using Java.Lang;
using Storm.Mvvm.Inject;
using String = System.String;
using Uri = Android.Net.Uri;

namespace IndiaRose.Services.Android
{
    public class MediaService : AbstractAndroidService,IMediaService
    {

        private MediaRecorder _recorder;
        private String _url;

        public void RecordSound()
        {
            _url = "/sdcard/IndiaRose/sound/test.3gpp";
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
            _recorder.Reset();

            return _url;
        }

        public string Camera()
        {
            string path = string.Format("sdcard/IndiaRose/image/IndiaRose_photo_{0}.jpg", Guid.NewGuid());
            File file = new File(path);

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(file));
            CurrentActivity.StartActivityForResult(intent, 0);
            return path;

        }

        public string StopRead(System.Uri data)
        {
            throw new System.NotImplementedException();
        }

       /* protected void Initialize(string path)
        {
            var imageIntent = new Intent();
            //chemin fonctionel sur nexus
            imageIntent.SetType(path);
            imageIntent.SetAction(Intent.ActionGetContent);
            //SartActivityForResult attend resultat
            StartActivityForResult(
            Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }


        //result de SartActivityForResult a tester une fois binde
        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            //a voir
            base.OnActivityResult(requestCode, resultCode, data);

            //verif result activity
            if (resultCode == Result.Ok)
            {
                StopRead(data.Data.ToString());
            }
        }

        public string StopRead(string data)
        {
            //set axml
            return data;
        }
        /*
         * 
         * 
         * TODO Recuperer le chemin de l'image a la fin en utilisant StopRead();
         * 
         * 
         * 
         */


        public void StartRead(string path)
        {
            throw new NotImplementedException();
        }

        public string StopRead(string data)
        {
            throw new NotImplementedException();
        }

        public MediaService(IContainer container) : base(container)
        {
        }
    }
}