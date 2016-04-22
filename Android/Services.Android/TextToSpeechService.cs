using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Speech.Tts;
using Android.Util;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Java.Util;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;
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

		private int _readingCountStack;
		public bool IsReading { get { return _readingCountStack > 0; } }

		private TextToSpeech _speakerSpeech;
		private readonly Dictionary<string, string> _registeredSounds = new Dictionary<string, string>();
		private readonly ManualResetEvent _initMutex = new ManualResetEvent(false);

		public TextToSpeechService()
		{
			ActivityService.ActivityChanged += InitTts;
			//ActivityService.ActivityChanging += ReleaseTts;
		}

		public void OnInit(OperationResult status)
		{
			if (_speakerSpeech == null)
			{
				Task.Run(() =>
				{
					_initMutex.WaitOne();
					LazyResolver<IDispatcherService>.Service.InvokeOnUIThread(() => OnInit(status));
				});

				return;
			}
			_speakerSpeech.SetLanguage(Locale.Default);
			_speakerSpeech.SetOnUtteranceCompletedListener(this);

			Dictionary<string, string> speakParameters = new Dictionary<string, string>
			{
				{TextToSpeech.Engine.KeyParamUtteranceId, INITIALIZE_UTTERANCE_ID}
			};

			string word = "india rose";
#if __ANDROID_11__
			if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.Honeycomb)
			{
				word = "a";
				speakParameters.Add(TextToSpeech.Engine.KeyParamVolume, "0");
			}
#endif
			_speakerSpeech.Speak(word, QueueMode.Add, speakParameters);
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
#if __ANDROID_17__
			if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.JellyBeanMr1)
			{
				_speakerSpeech = new TextToSpeech(ActivityService.CurrentActivity.ApplicationContext, this);
				_initMutex.Set();
				return;
			}
#endif

			ActivityService.StartActivityForResult(new Intent().SetAction(TextToSpeech.Engine.ActionCheckTtsData),
				(result, data) =>
				{
					if (result == Result.Ok)
					{
						// success, create the TTS instance
						_speakerSpeech = new TextToSpeech(ActivityService.CurrentActivity, this);
						_initMutex.Set();
					}
					else
					{
						Intent installIntent = new Intent();
						installIntent.SetAction(TextToSpeech.Engine.ActionInstallTtsData);
						ActivityService.CurrentActivity.StartActivity(installIntent);
					}
				});
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
                    _speakerSpeech.AddSpeech(word, indiagram.SoundPath);
                }
				else
				{
					if (_speakerSpeech != null)
					{
						word = Guid.NewGuid().ToString();
						_registeredSounds.Add(indiagram.SoundPath, word);
						_speakerSpeech.AddSpeech(word, indiagram.SoundPath);
					}
					else
					{
						//TODO : log issue
						word = "e";
					}
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
			if (_speakerSpeech == null)
			{
				//TODO : log issue
				this.RaiseEvent(SpeakingCompleted);
				return;
			}

			Log.Error("TTS", "Playing indiagram {0}", text);
			_readingCountStack++;
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
			_readingCountStack--;
			this.RaiseEvent(SpeakingCompleted);
		}
	}
}