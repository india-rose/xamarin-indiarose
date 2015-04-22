using System;
using Android.Media;
using IndiaRose.Interfaces;

namespace IndiaRose.Services.Android
{
    public class MediaService : IMedia
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