using System;
using Java.IO;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Provider;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Environment= Android.OS.Environment;
using Storm.Mvvm.Interfaces;
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

        public string Camera()
        {
            string path = string.Format(Environment.ExternalStorageDirectory.Path+"/IndiaRose/image/IndiaRose_photo_{0}.jpg", Guid.NewGuid());
            File file = new File(path);

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(file));
            ActivityService.StartActivityForResult(intent, callback);
            return path;

        }

        private void callback(Result arg1, Intent arg2)
        {
            return ;
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
    }
}