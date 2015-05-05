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

        private string Text;
        private TextToSpeech speakerSpeech;

        public void ReadText(string text)
        {
            Text = text;
            speakerSpeech=new TextToSpeech(ActivityService.CurrentActivity,this);
            speakerSpeech.SetLanguage(Locale.Default);
        }

        public void OnInit(OperationResult status)
        {
            speakerSpeech.Speak(Text, QueueMode.Add, null);
        }
    }
}