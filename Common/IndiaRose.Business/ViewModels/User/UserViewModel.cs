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

namespace IndiaRose.Business.ViewModels.User
{
	public class UserViewModel : AbstractViewModel
	{
		private readonly object _readingMutex = new object();
		private readonly ManualResetEvent _sentenceReadingSemaphore = new ManualResetEvent(false);
		private readonly Category _correctionCategory;
		private readonly Stack<Category> _navigationStack = new Stack<Category>();
		private readonly ObservableCollection<IndiagramUIModel> _sentenceIndiagrams = new ObservableCollection<IndiagramUIModel>(); 

		private int _collectionOffset;
		private int _collectionDisplayCount;
		private List<Indiagram> _collectionIndiagrams;
		private Category _currentCategory;
		private bool _isReading;
		private bool _isCorrectionModeEnabled;
		private bool _sentenceCanAddMoreIndiagrams = true;
		private int _sentenceIndiagramId = -242;

		#region Services

		public ISettingsService SettingsService
		{
			get { return LazyResolver<ISettingsService>.Service; }
		}

		protected ICollectionStorageService CollectionStorageService
		{
			get { return LazyResolver<ICollectionStorageService>.Service; }
		}

		protected IStorageService StorageService
		{
			get { return LazyResolver<IStorageService>.Service; }
		}

		protected ITextToSpeechService TextToSpeechService
		{
			get { return LazyResolver<ITextToSpeechService>.Service; }
		}

		#endregion

		#region Properties

		public Category CurrentCategory
		{
			get { return _currentCategory; }
			set
			{
				if (SetProperty(ref _currentCategory, value) && value != null)
				{
					RefreshDisplayList();
				}
			}
		}

		public List<Indiagram> CollectionIndiagrams
		{
			get { return _collectionIndiagrams; }
			set { SetProperty(ref _collectionIndiagrams, value); }
		}

		public int CollectionOffset
		{
			get { return _collectionOffset; }
			set { SetProperty(ref _collectionOffset, value); }
		}

		public int CollectionDisplayCount
		{
			get { return _collectionDisplayCount; }
			set { SetProperty(ref _collectionDisplayCount, value); }
		}

		public ObservableCollection<IndiagramUIModel> SentenceIndiagrams
		{
			get { return _sentenceIndiagrams; }
		}

		public bool SentenceCanAddMoreIndiagrams
		{
			get { return _sentenceCanAddMoreIndiagrams; }
			set { SetProperty(ref _sentenceCanAddMoreIndiagrams, value); }
		}

		public bool IsCorrectionModeEnabled
		{
			get { return _isCorrectionModeEnabled; }
			set { SetProperty(ref _isCorrectionModeEnabled, value); }
		}

		#endregion

		#region ICommand

		public ICommand CollectionIndiagramSelectedCommand { get; private set; }
		public ICommand SentenceIndiagramSelectedCommand { get; private set; }
		public ICommand EnterCorrectionModeCommand { get; private set; }
		public ICommand CollectionNextCommand { get; private set; }
		public ICommand ReadSentenceCommand { get; private set; }
		public ICommand CollectionIndiagramDragStartCommand { get; private set; }


		#endregion

		public UserViewModel()
		{
			Category rootCategory = new Category(CollectionStorageService.Collection)
			{
				Id = -1,
				Text = LocalizationService.GetString("Collection_RootCategoryName", "Text"),
				ImagePath = StorageService.ImageRootPath,
			};

			_correctionCategory = new Category
			{
				Id = -2,
				Text = LocalizationService.GetString("Collection_CorrectionCategoryName", "Text"),
				ImagePath = StorageService.ImageCorrectionPath
			};

			CollectionIndiagramSelectedCommand = new DelegateCommand<Indiagram>(CollectionIndiagramSelectedAction);
			CollectionIndiagramDragStartCommand = new DelegateCommand<Indiagram>(CollectionIndiagramDragStartAction);
			SentenceIndiagramSelectedCommand = new DelegateCommand<IndiagramUIModel>(SentenceIndiagramSelectedAction);
			EnterCorrectionModeCommand = new DelegateCommand(EnterCorrectionModeAction);
			CollectionNextCommand = new DelegateCommand(CollectionNextAction);
			ReadSentenceCommand = new DelegateCommand(ReadSentenceAction);

			PushCategory(rootCategory);

			TextToSpeechService.SpeakingCompleted += OnIndiagramReadCompleted;
		}

		#region Collection navigation

		private void CollectionNextAction()
		{
			int offset = CollectionOffset;
			offset += CollectionDisplayCount;

			if (offset >= CollectionIndiagrams.Count)
			{
				if (_navigationStack.Count > 1)
				{
					PopCategory();
				}
				else
				{
					CollectionOffset = 0;
				}
			}
			else
			{
				CollectionOffset = offset;
			}
		}

		private void PushCategory(Category category)
		{
			_navigationStack.Push(category);
			CollectionOffset = 0;
			CurrentCategory = category;
		}
		
		private bool PopCategory()
		{
			if (_navigationStack.Count <= 1) //always keep root category on the stack
			{
				return false;
			}

			_navigationStack.Pop();
			CollectionOffset = 0;
			CurrentCategory = _navigationStack.Peek();

			return true;
		}

		private void RefreshDisplayList()
		{
			if (_navigationStack.Count == 0)
			{
				return;
			}

			CollectionIndiagrams = CurrentCategory.Children.Where(
				SettingsService.IsMultipleIndiagramSelectionEnabled ? 
					(Func<Indiagram, bool>)(x => x.IsEnabled) : 
					(x => x.IsEnabled && SentenceIndiagrams.All(y => y.Model.Id != x.Id))).ToList();
		}

		#endregion

		private void EnterCorrectionModeAction()
		{
			if (SentenceIndiagrams.Count == 0)
			{
				return;
			}

			if (CheckIsReading())
			{
				return;
			}

			_correctionCategory.Children.Clear();
			foreach (IndiagramUIModel indiagramUI in SentenceIndiagrams)
			{
				_correctionCategory.Children.Add(indiagramUI.Model);
			}
			
			PushCategory(_correctionCategory);
			IsCorrectionModeEnabled = true;
		}

		private void SentenceIndiagramSelectedAction(IndiagramUIModel indiagram)
		{
			lock (_readingMutex)
			{
				if (_isReading)
				{
					return;
				}
			}

			SentenceIndiagrams.Remove(indiagram);
			if (!SettingsService.IsMultipleIndiagramSelectionEnabled)
			{
				RefreshDisplayList();
			}
		}

		private void CollectionIndiagramSelectedAction(Indiagram indiagram)
		{
			if (CheckIsReading() && (indiagram.IsCategory || !SettingsService.IsDragAndDropEnabled))
			{
				return;
			}

			Category category = indiagram as Category;
			if (category != null)
			{
				Read(indiagram);
				PushCategory(category);
			}
			else
			{
				if (SentenceCanAddMoreIndiagrams)
				{
					if (!SettingsService.IsDragAndDropEnabled)
					{
						Read(indiagram);
					}
					AddIndiagramToSentence(indiagram);

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

		private void CollectionIndiagramDragStartAction(Indiagram indiagram)
		{
			if (CheckIsReading())
			{
				return;
			}

			Read(indiagram);
		}


		private void Read(Indiagram indiagram)
		{
			bool canRead = true;
			if (indiagram.IsCategory)
			{
				canRead = SettingsService.IsCategoryNameReadingEnabled;
			}

			if (canRead)
			{
				lock (_readingMutex)
				{
					if (_isReading)
					{
						canRead = false;
					}
					else
					{
						_isReading = true;
					}
				}
			}

			if (canRead)
			{
				TextToSpeechService.PlayIndiagram(indiagram);
			}
		}

		private void OnIndiagramReadCompleted(object sender, EventArgs eventArgs)
		{
			lock (_readingMutex)
			{
				_isReading = false;
			}
		}


		#region Read sentence handling part

		private void AddIndiagramToSentence(Indiagram indiagram)
		{
			if (SettingsService.IsMultipleIndiagramSelectionEnabled)
			{
				// need to create a copy of the indiagram
				Indiagram copy = new Indiagram();
				copy.CopyFrom(indiagram);
				indiagram = copy;
				indiagram.Id = _sentenceIndiagramId--;
			}

			SentenceIndiagrams.Add(new IndiagramUIModel(indiagram));
		}

		private void ReadSentenceAction()
		{
			//if there is no indiagram to read do nothing
			if (SentenceIndiagrams.Count == 0)
			{
				return;
			}

			bool canRead = false;
			lock (_readingMutex)
			{
				if (!_isReading)
				{
					canRead = true;
					_isReading = true;
				}
			}
			if (!canRead)
			{
				return;
			}
			TextToSpeechService.SpeakingCompleted -= OnIndiagramReadCompleted;
			TextToSpeechService.SpeakingCompleted += OnSentenceIndiagramReadCompleted;

			Task.Run((Action)ReadSentence);
		}

		private async void ReadSentence()
		{
			bool isReinforcerEnabled = SettingsService.IsReinforcerEnabled;
			foreach (IndiagramUIModel sentenceIndiagram in SentenceIndiagrams)
			{
				IndiagramUIModel currentIndiagram = sentenceIndiagram;
				//enable reinforcer
				if (isReinforcerEnabled)
				{
					DispatcherService.InvokeOnUIThread(() => currentIndiagram.IsReinforcerEnabled = true);
				}

				// read indiagram and wait for reading to finished
				TextToSpeechService.PlayIndiagram(sentenceIndiagram.Model);
				_sentenceReadingSemaphore.WaitOne();

				// wait delay specified in settings before going to next one
				int millisecondsToWait = (int)(SettingsService.TimeOfSilenceBetweenWords * 1000);
				if (millisecondsToWait > 10)
				{
					await Task.Delay(millisecondsToWait);
				}

				// disable reinforcer
				if (isReinforcerEnabled)
				{
					DispatcherService.InvokeOnUIThread(() =>currentIndiagram.IsReinforcerEnabled = false);
				}
			}

			_sentenceIndiagramId = -242;
			// rewire events correctly
			TextToSpeechService.SpeakingCompleted -= OnSentenceIndiagramReadCompleted;
			TextToSpeechService.SpeakingCompleted += OnIndiagramReadCompleted;

			lock (_readingMutex)
			{
				_isReading = false;
			}

			DispatcherService.InvokeOnUIThread(() =>
			{
				IsCorrectionModeEnabled = false;
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

		private void OnSentenceIndiagramReadCompleted(object sender, EventArgs eventArgs)
		{
			_sentenceReadingSemaphore.Set();
		}

		#endregion

		#region Reading freezing input part

		private bool CheckIsReading()
		{
			lock (_readingMutex)
			{
				return _isReading;
			}
		}

		#endregion
	}
}
