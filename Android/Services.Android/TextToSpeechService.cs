using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech.Tts;
using Android.Util;
using Android.Widget;
using IndiaRose.Interfaces;
using Java.Util;
using Storm.Mvvm.Events;
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

        private TextToSpeech _speakerSpeech;

        public TextToSpeechService()
        {
            var currentapiVersion = Build.VERSION.SdkInt;
            if (currentapiVersion >= BuildVersionCodes.JellyBeanMr1)
            {
                _speakerSpeech = new TextToSpeech(ActivityService.CurrentActivity, this);
                //this.m_stateManager.start();
                //si besoin voir code java
            }
            else
            {
                ActivityService.ActivityChanged+=InitTTS;
            }
        }

        public void ReadText(string text)
        {
            _speakerSpeech.Speak(text, QueueMode.Add, null);
        }

        public void OnInit(OperationResult status)
        {
            _speakerSpeech.SetLanguage(Locale.Default);
        }

        //todo a tester sur faible api
        private void InitTTS(object sender, ValueChangedEventArgs<Activity> valueChangedEventArgs)
        {
            ActivityService.StartActivityForResult(new Intent().SetAction(TextToSpeech.Engine.ActionCheckTtsData), (result, data) =>
                {
                    if (result == Result.Ok)
                    {
                        // success, create the TTS instance
                        _speakerSpeech = new TextToSpeech(ActivityService.CurrentActivity, this);
                    }
                    else
                    {
                        Intent installIntent = new Intent();
                        installIntent.SetAction(TextToSpeech.Engine.ActionInstallTtsData);
                        ActivityService.CurrentActivity.StartActivity(installIntent);

                    }
                });
        }
    }
}