using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech.Tts;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Java.Util;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;
using Object = Java.Lang.Object;

namespace IndiaRose.Services.Android
{
	public class TextToSpeechService : Object, ITextToSpeechService, TextToSpeech.IOnInitListener
	{
		public event EventHandler SpeakingCompleted;

		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		protected ILocalizationService LocalizationService
		{
			get { return LazyResolver<ILocalizationService>.Service; }
		}

		private TextToSpeech _speakerSpeech;
		private readonly Semaphore _runningSemaphore = new Semaphore(0, 100);
		private readonly Dictionary<string, string> _registeredSounds = new Dictionary<string, string>();

		public TextToSpeechService()
		{
			ActivityService.ActivityChanged += InitTts;
		}

		public void OnInit(OperationResult status)
		{
			_speakerSpeech.SetLanguage(Locale.Default);
			RunSpeakerChecking();
		}

		//todo a tester sur faible api
		private void InitTts(object sender, ValueChangedEventArgs<Activity> valueChangedEventArgs)
		{
			var currentapiVersion = Build.VERSION.SdkInt;
			if (currentapiVersion >= BuildVersionCodes.JellyBeanMr1)
			{
				_speakerSpeech = new TextToSpeech(ActivityService.CurrentActivity, this);
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

		private void RunSpeakerChecking()
		{
			new Thread(() =>
			{
				while (true)
				{
					LazyResolver<ILoggerService>.Service.Log("====> TTS : Waiting for semaphore !", MessageSeverity.Debug);
					_runningSemaphore.WaitOne();
					LazyResolver<ILoggerService>.Service.Log("====> TTS : Waiting in non speaking mode !", MessageSeverity.Debug);
					while (_speakerSpeech != null && !_speakerSpeech.IsSpeaking)
					{
						Thread.Yield();
					}

					LazyResolver<ILoggerService>.Service.Log("====> TTS : Waiting in speaking mode !", MessageSeverity.Debug);
					while (_speakerSpeech != null && _speakerSpeech.IsSpeaking)
					{
						Thread.Yield();
					}

					LazyResolver<ILoggerService>.Service.Log("====> TTS : Finish detection of a word !", MessageSeverity.Debug);

					if (_speakerSpeech == null)
					{
						LazyResolver<ILoggerService>.Service.Log("====> TTS : breakout exiting process !", MessageSeverity.Debug);
						break;
					}
					LazyResolver<ILoggerService>.Service.Log("====> TTS : raising completed event !", MessageSeverity.Debug);
					this.RaiseEvent(SpeakingCompleted);
				}
			}).Start();
		}

		public void Close()
		{
			_speakerSpeech.Stop();
			_speakerSpeech.Shutdown();
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
			_runningSemaphore.Release();
			_speakerSpeech.Speak(text, QueueMode.Add, null);
		}
	}
}