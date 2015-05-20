using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.User
{
	public class UserHomeViewModel : AbstractBrowserViewModel
	{
		#region Services

		protected IXmlService XmlService
		{
			get { return LazyResolver<IXmlService>.Service; }
		}

		protected IResourceService ResourceService
		{
			get { return LazyResolver<IResourceService>.Service; }
		}

		protected ITextToSpeechService TtsService
		{
			get { return LazyResolver<ITextToSpeechService>.Service; }
		}

		#endregion

		private readonly object _lockMutex = new object();
		private bool _initialized;
		private readonly Semaphore _readSemaphore = new Semaphore(0, 1);

		private bool _isReading;

		private ObservableCollection<IndiagramUIModel> _sentenceIndiagrams;
		private bool _canAddMoreIndiagrams = true;

		public string BotBackgroundColor
		{
			get { return SettingsService.BottomBackgroundColor; }
		}

		public ObservableCollection<IndiagramUIModel> SentenceIndiagrams
		{
			get { return _sentenceIndiagrams; }
			set { SetProperty(ref _sentenceIndiagrams, value); }
		}

		public bool CanAddMoreIndiagrams
		{
			get { return _canAddMoreIndiagrams; }
			set { SetProperty(ref _canAddMoreIndiagrams, value); }
		}

		public ICommand ReadSentenceCommand { get; private set; }
		public ICommand SentenceIndiagramSelectedCommand { get; private set; }

		public UserHomeViewModel()
		{
			SentenceIndiagrams = new ObservableCollection<IndiagramUIModel>();
			ReadSentenceCommand = new DelegateCommand(ReadSentenceAction);
			SentenceIndiagramSelectedCommand = new DelegateCommand<Indiagram>(SentenceIndiagramSelectedAction);

			TtsService.SpeakingCompleted += OnTtsSpeakingCompleted;
		}

		private void OnTtsSpeakingCompleted(object sender, EventArgs eventArgs)
		{
			lock (_lockMutex)
			{
				if (_isReading)
				{
					_isReading = false;
				}
			}
		}

		#region Collection import in case of first launch

		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			base.OnNavigatedTo(e, parametersKey);

			lock (_lockMutex)
			{
				CollectionStorageService.Initialized += (sender, args) =>
				{
					lock (_lockMutex)
					{
						OnCollectionInitialized();
					}
				};
				if (CollectionStorageService.IsInitialized)
				{
					OnCollectionInitialized();
				}
			}
		}

		private async void OnCollectionInitialized()
		{
			if (_initialized)
			{
				return;
			}
			_initialized = true;

			if (CollectionStorageService.Collection.Count == 0)
			{
				if (await XmlService.HasOldCollectionFormatAsync())
				{
					DispatcherService.InvokeOnUIThread(() =>
						MessageDialogService.Show(Dialogs.IMPORTING_COLLECTION, new Dictionary<string, object>
						{
							{"MessageUid", "ImportCollection_FromOldFormat"}
						}));

					LoggerService.Log("==> Importing collection from old format");
					await XmlService.InitializeCollectionFromOldFormatAsync();
					LoggerService.Log("# Import finished");
				}
				else
				{
					DispatcherService.InvokeOnUIThread(() =>
						MessageDialogService.Show(Dialogs.IMPORTING_COLLECTION, new Dictionary<string, object>
						{
							{"MessageUid", "ImportCollection_FromZip"}
						}));

					LoggerService.Log("==> Importing collection from zip file");
					await XmlService.InitializeCollectionFromZipStreamAsync(ResourceService.OpenZip("indiagrams.zip"));
				}
				LoggerService.Log("# Import finished");
				MessageDialogService.DismissCurrentDialog();
			}
		}

		#endregion

		#region Bottom parts action

		private void SentenceIndiagramSelectedAction(Indiagram indiagram)
		{
			lock (_lockMutex)
			{
				if (!_isReading)
				{
					SentenceIndiagrams.Remove(SentenceIndiagrams.FirstOrDefault(x => Indiagram.AreSameIndiagram(indiagram, x.Model)));
				}
			}
		}

		private void ReadSentenceAction()
		{
			bool canRead = false;
			lock (_lockMutex)
			{
				if (!_isReading)
				{
					canRead = true;
					_isReading = true;
				}
			}
			if (canRead)
			{
				TtsService.SpeakingCompleted -= OnTtsSpeakingCompleted;
				TtsService.SpeakingCompleted += OnWordReadingCompleted;
				Task.Run((Action)ReadSentence);
			}
		}

		private void OnWordReadingCompleted(object sender, EventArgs e)
		{
			_readSemaphore.Release();
		}

		private async void ReadSentence()
		{
			bool reinforcer = SettingsService.IsReinforcerEnabled;
			foreach (IndiagramUIModel currentIndiagram in SentenceIndiagrams)
			{
				if (reinforcer)
				{
					var indiagram = currentIndiagram;
					DispatcherService.InvokeOnUIThread(() => indiagram.IsReinforcerEnabled = true);
				}

				// read indiagram
				TtsService.PlayIndiagram(currentIndiagram.Model);

				// wait for tts to finished
				_readSemaphore.WaitOne();

				// wait for some seconds (settings reading delay)
				int millisecondsToWait = (int)(SettingsService.TimeOfSilenceBetweenWords*1000);
				if (millisecondsToWait > 10)
				{
					await Task.Delay(millisecondsToWait);
				}

				// disable reinforcer
				if (reinforcer)
				{
					var indiagram = currentIndiagram;
					DispatcherService.InvokeOnUIThread(() => indiagram.IsReinforcerEnabled = false);
				}
			}

			// at the end of the sentence : 
			//	resume interactions & others needed
			//	relink event on Tts

			TtsService.SpeakingCompleted -= OnWordReadingCompleted;
			TtsService.SpeakingCompleted += OnTtsSpeakingCompleted;

			lock (_lockMutex)
			{
				_isReading = false;
			}

			DispatcherService.InvokeOnUIThread(() =>
			{
				SentenceIndiagrams.Clear();
				if (PopCategory())
				{
					while (PopCategory())
					{
					}
				}
				else
				{
					RefreshDisplayList();
				}
			});
		}

		#endregion

		#region Top parts action

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			lock (_lockMutex)
			{
				if (_isReading)
				{
					return;
				}
			}
			// Indiagram has been selected from top
			if (indiagram.IsCategory)
			{
				if (SettingsService.IsCategoryNameReadingEnabled)
				{
					lock (_lockMutex)
					{
						_isReading = true;
					}
					TtsService.PlayIndiagram(indiagram);
				}
				PushCategory((Category) indiagram);
			}
			else
			{
				if (CanAddMoreIndiagrams)
				{
					lock (_lockMutex)
					{
						_isReading = true;
					}
					TtsService.PlayIndiagram(indiagram);
					SentenceIndiagrams.Add(new IndiagramUIModel(indiagram));
					if (SettingsService.IsBackHomeAfterSelectionEnabled && PopCategory())
					{
						while (PopCategory())
						{
						}
					}
					else
					{
						RefreshDisplayList();
					}
				}
			}
		}

		protected override IEnumerable<Indiagram> FilterCollection(IEnumerable<Indiagram> input)
		{
			return input.Where(indiagram =>
				indiagram.IsEnabled &&
				SentenceIndiagrams.FirstOrDefault(x => Indiagram.AreSameIndiagram(x.Model, indiagram)) == null);
		}

		#endregion
	}
}