using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.PlayTo;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using IndiaRose.Interfaces;
using IndiaRose.Data.Model;
using Storm.Mvvm.Inject;

namespace IndiaRose.Services
{
	public class TextToSpeechService : ITextToSpeechService
	{
	    private readonly SpeechSynthesizer _ttsSpeechSynthesizer=new SpeechSynthesizer();
        // The media object for controlling and playing audio.
        private MediaElement _sound;

	    public event EventHandler SpeakingCompleted;

	    public void Close()
        {
            _sound.Stop();
            _ttsSpeechSynthesizer.Dispose();
		}

		public async void PlayIndiagram(Indiagram indiagram)
		{
            if (indiagram.HasCustomSound)
            {
                var audioFile = await StorageFile.GetFileFromPathAsync(indiagram.SoundPath);
                var stream = await audioFile.OpenAsync(FileAccessMode.Read);
                PlayStream(stream,"");
            }
            else
            {
                SpeechSynthesisStream stream = await _ttsSpeechSynthesizer.SynthesizeTextToStreamAsync(indiagram.Text);
                PlayStream(stream, stream.ContentType);
            }
		}

	    private void PlayStream(IRandomAccessStream stream, string mimetype)
	    {
            if (_sound==null||_sound.CurrentState != MediaElementState.Playing)
            {
                _sound = new MediaElement();
                _sound.MediaEnded += _sound_MediaEnded;
                _sound.SetSource(stream,mimetype);
                _sound.Play();
            }
	    }

        private void _sound_MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SpeakingCompleted != null)
                SpeakingCompleted(this, null);
        }
	}
}
