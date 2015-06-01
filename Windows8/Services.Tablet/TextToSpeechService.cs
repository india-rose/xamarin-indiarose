using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.PlayTo;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
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
		public async void ReadText(string text)
		{

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await _ttsSpeechSynthesizer.SynthesizeTextToStreamAsync(text);

            // Send the stream to the media object.
            if (_sound != null)
            {
                _sound.Stop();
            }
                _sound = new MediaElement();
                _sound.SetSource(stream, stream.ContentType);            
                _sound.Play();
		}

	    public event EventHandler SpeakingCompleted;

	    public void Close()
		{
            _ttsSpeechSynthesizer.Dispose();
		}

		public void PlayIndiagram(Indiagram indiagram)
		{
            if (indiagram.HasCustomSound)
            {
                LazyResolver<IMediaService>.Service.PlaySound(indiagram.SoundPath);
            }
            else
            {
                ReadText(indiagram.Text);
            }
		}
	}
}
