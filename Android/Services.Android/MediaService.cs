using System;
using Android.Content;
using Android.Media;
using IndiaRose.Interfaces;

namespace IndiaRose.Services.Android
{
    public class MediaService : IMediaService
    {

        private MediaRecorder _recorder;
        private String _url;

        public void StartWrite(string path)
        {
            _url = "/sdcard/IndiaRose/sound/" + path + ".3gpp";
            _recorder = new MediaRecorder();
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            _recorder.SetOutputFile(_url);
            _recorder.Prepare();
            _recorder.Start();
        }

        public string StopWrite()
        {
            _recorder.Stop();
            _recorder.Reset();

            return _url;
        }

        public string StopRead(System.Uri data)
        {
            throw new System.NotImplementedException();
        }

        protected void Initialize(string path)
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
    }
}