using System;
using Android.App;
using Android.Content;
using Android.Speech.Tts;
using Android.Widget;
using IndiaRose.Interfaces;
using Java.Util;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;

namespace IndiaRose.Services.Android
{
    public class TextToSpeechService : Java.Lang.Object, ITextToSpeechService, TextToSpeech.IOnInitListener
    {
        protected IActivityService ActivityService
        {
            get { return LazyResolver<IActivityService>.Service; }
        }
        protected ILocalizationService LocalizationService
        {
            get { return LazyResolver<ILocalizationService>.Service; }
        }

        private readonly TextToSpeech _speakerSpeech;

        public TextToSpeechService()
        {
            _speakerSpeech = new TextToSpeech(ActivityService.CurrentActivity, this);
        }

        public void ReadText(string text)
        {
            _speakerSpeech.Speak(text, QueueMode.Add, null);
        }

        public void OnInit(OperationResult status)
        {
            _speakerSpeech.SetLanguage(Locale.Default);
        }
    }
}