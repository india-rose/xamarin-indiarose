using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech.Tts;
using Android.Util;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Java.Util;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Object = Java.Lang.Object;

// disable deprecated use because of supporting older api versions
#pragma warning disable 618

namespace IndiaRose.Services.Android
{
	public class TextToSpeechService : Object, ITextToSpeechService, TextToSpeech.IOnInitListener, TextToSpeech.IOnUtteranceCompletedListener
	{
		private const string INITIALIZE_UTTERANCE_ID = "Storm0x2a";
		public event EventHandler SpeakingCompleted;
		public event EventHandler Initialized;

		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		private TextToSpeech _speakerSpeech;
		private readonly Dictionary<string, string> _registeredSounds = new Dictionary<string, string>();

		public TextToSpeechService()
		{
			ActivityService.ActivityChanged += InitTts;
			//ActivityService.ActivityChanging += ReleaseTts;
		}

		public void OnInit(OperationResult status)
		{
			_speakerSpeech.SetLanguage(Locale.Default);
			_speakerSpeech.SetOnUtteranceCompletedListener(this);

			_speakerSpeech.Speak("india rose", QueueMode.Add, new Dictionary<string, string>
			{
				{TextToSpeech.Engine.KeyParamUtteranceId, INITIALIZE_UTTERANCE_ID},
				{TextToSpeech.Engine.KeyParamVolume, "0"}
			});
			Log.Error("TTS", "Engine initialized");
		}

		//todo a tester sur faible api
		private void InitTts(object sender, ValueChangedEventArgs<Activity> valueChangedEventArgs)
		{
			if (_speakerSpeech == null)
			{
				Initialize();
			}
		}

		private void Initialize()
		{
			Log.Error("TTS", "Initialize TTS engine");
			BuildVersionCodes currentapiVersion = Build.VERSION.SdkInt;
			if (currentapiVersion >= BuildVersionCodes.JellyBeanMr1)
			{
				_speakerSpeech = new TextToSpeech(ActivityService.CurrentActivity.ApplicationContext, this);
			}
			else
			{
				ActivityService.StartActivityForResult(new Intent().SetAction(TextToSpeech.Engine.ActionCheckTtsData),
					(result, data) =>
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

		private void ReleaseTts(object sender, ValueChangingEventArgs<Activity> valueChangingEventArgs)
		{
			Close();
		}
		
		public void Close()
		{
			if (_speakerSpeech == null)
			{
				return;
			}
			Log.Error("TTS", "TTS engine shutting down");
			_speakerSpeech.Stop();
			_speakerSpeech.Shutdown();
			_speakerSpeech.Dispose();
			_speakerSpeech = null;
		}

		public void PlayIndiagram(Indiagram indiagram)
		{
			if (indiagram.HasCustomSound)
			{
				string word;
				if (_registeredSounds.ContainsKey(indiagram.SoundPath))
				{
					word = _registeredSounds[indiagram.SoundPath];
				}
				else
				{
					word = Guid.NewGuid().ToString();
					_registeredSounds.Add(indiagram.SoundPath, word);
					_speakerSpeech.AddSpeech(word, indiagram.SoundPath);
				}
				PlayText(word);
			}
			else
			{
				PlayText(indiagram.Text);
			}
		}

		protected void PlayText(string text)
		{
			Log.Error("TTS", "Playing indiagram {0}", text);
			_speakerSpeech.Speak(text, QueueMode.Add, new Dictionary<string, string>
			{
				{TextToSpeech.Engine.KeyParamUtteranceId, Guid.NewGuid().ToString()},
			});
		}

		public void OnUtteranceCompleted(string utteranceId)
		{
			Log.Error("TTS", "Utterance completed");
			if (INITIALIZE_UTTERANCE_ID.Equals(utteranceId))
			{
				Log.Error("TTS", "Initialize Utterance received !");
				this.RaiseEvent(Initialized);
				return;
			}

			this.RaiseEvent(SpeakingCompleted);
		}
	}
}