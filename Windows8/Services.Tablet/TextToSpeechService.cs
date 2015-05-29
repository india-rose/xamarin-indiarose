using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
	public class TextToSpeechService : ITextToSpeechService
	{
	    private readonly SpeechSynthesizer _ttsSpeechSynthesizer=new SpeechSynthesizer();
        private MediaElement _sound;
		public async void ReadText(string text)
		{
            // The media object for controlling and playing audio.
            MediaElement mediaElement = new MediaElement();

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await _ttsSpeechSynthesizer.SynthesizeTextToStreamAsync(text);

            // Send the stream to the media object.
            if (_sound == null)
            {
                _sound= new MediaElement();
                _sound.SetSource(stream, stream.ContentType);            
            }
                _sound.Stop();
                _sound = new MediaElement();
                _sound.SetSource(stream, stream.ContentType);            
                _sound.Play();
		}

		public void Close()
		{
            _ttsSpeechSynthesizer.Dispose();
		}
	}
}
