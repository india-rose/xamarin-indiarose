using System;
using System.IO;
using Windows.Media.Core;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using Storm.Mvvm.Events;

namespace Services.Tablet
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly SpeechSynthesizer _ttsSpeechSynthesizer = new SpeechSynthesizer();

        public event EventHandler SpeakingCompleted;

        public TextToSpeechService()
        {
            SoundUtilities.Completed += (sender, args) => this.RaiseEvent(SpeakingCompleted);
        }

        //todo
        public bool IsReading { get; private set; }

        public void Close()
        {
            _ttsSpeechSynthesizer.Dispose();
        }

        public async void PlayIndiagram(Indiagram indiagram)
        {
            if (indiagram.HasCustomSound)
            {
                var audioFile = await StorageFile.GetFileFromPathAsync(indiagram.SoundPath);

                if (audioFile != null)
                {
                    MediaElement mediaElement = new MediaElement();

                    var mediaSource = MediaSource.CreateFromStorageFile(audioFile);
                    mediaElement.SetPlaybackSource(mediaSource);
                    mediaElement.Play();
                }
            }
            else
            {
                SpeechSynthesisStream stream = await _ttsSpeechSynthesizer.SynthesizeTextToStreamAsync(indiagram.Text);
                PlayStream(stream);
            }
        }

        private void PlayStream(IRandomAccessStream stream)
        {
            SoundUtilities.PlaySound(stream.AsStream());
        }
    }
}
