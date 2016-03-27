using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.PlayTo;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using IndiaRose.Interfaces;
using IndiaRose.Data.Model;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;

namespace IndiaRose.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly SpeechSynthesizer _ttsSpeechSynthesizer = new SpeechSynthesizer();

        public event EventHandler SpeakingCompleted;

        public TextToSpeechService()
        {
            SoundUtilities.Completed += (sender, args) => this.RaiseEvent(SpeakingCompleted);
        }

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
                var stream = await audioFile.OpenAsync(FileAccessMode.Read);
                PlayStream(stream);
            }
            else
            {
                SpeechSynthesisStream stream = await _ttsSpeechSynthesizer.SynthesizeTextToStreamAsync(indiagram.Text);
                PlayStream(stream);
            }
        }

        private void PlayStream(IRandomAccessStream stream)
        {
            //SoundUtilities.PlaySound(stream.AsStream());
        }
    }
}
