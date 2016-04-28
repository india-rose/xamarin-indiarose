using System;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using Storm.Mvvm.Events;

namespace Services.Tablet
{
    public class TextToSpeechService : ITextToSpeechService
    {
        //private readonly SpeechSynthesizer _ttsSpeechSynthesizer = new SpeechSynthesizer();

        public event EventHandler SpeakingCompleted;

        public TextToSpeechService()
        {
            //SoundUtilities.Completed += (sender, args) => this.RaiseEvent(SpeakingCompleted);
        }

        //todo
        public bool IsReading { get; private set; }

        public void Close()
        {
            //_ttsSpeechSynthesizer.Dispose();
        }

        public async void PlayIndiagram(Indiagram indiagram)
        {
            /*if (indiagram.HasCustomSound)
            {
                var audioFile = await StorageFile.GetFileFromPathAsync(indiagram.SoundPath);
                var stream = await audioFile.OpenAsync(FileAccessMode.Read);
                PlayStream(stream);
            }
            else
            {
                SpeechSynthesisStream stream = await _ttsSpeechSynthesizer.SynthesizeTextToStreamAsync(indiagram.Text);
                PlayStream(stream);
            }*/
        }

        private void PlayStream(IRandomAccessStream stream)
        {
            //SoundUtilities.PlaySound(stream.AsStream());
        }
    }
}
