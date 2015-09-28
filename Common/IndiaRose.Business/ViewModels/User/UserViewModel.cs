using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
		private readonly Category _correctionCategory;
		private readonly Stack<Category> _navigationStack = new Stack<Category>();
		private readonly ObservableCollection<IndiagramUIModel> _sentenceIndiagrams = new ObservableCollection<IndiagramUIModel>(); 

		private int _collectionOffset;
		private int _collectionDisplayCount;
		private List<Indiagram> _collectionIndiagrams;
		private Category _currentCategory;
		private bool _isReading;
		private bool _sentenceCanAddMoreIndiagrams;

		#region Services

		protected ISettingsService SettingsService
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

		#endregion

		#region Properties

		public Category CurrentCategory
		{
			get { return _currentCategory; }
			set { SetProperty(ref _currentCategory, value); }
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

		#endregion

		#region ICommand

		public ICommand CollectionIndiagramSelectedCommand { get; private set; }
		public ICommand SentenceIndiagramSelectedCommand { get; private set; }
		public ICommand EnterCorrectionModeCommand { get; private set; }
		public ICommand CollectionNextCommand { get; private set; }
		public ICommand ReadSentenceCommand { get; private set; }

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
			SentenceIndiagramSelectedCommand = new DelegateCommand<IndiagramUIModel>(SentenceIndiagramSelectedAction);
			EnterCorrectionModeCommand = new DelegateCommand(EnterCorrectionModeAction);
			CollectionNextCommand = new DelegateCommand(CollectionNextAction);
			ReadSentenceCommand = new DelegateCommand(ReadSentenceAction);

			PushCategory(rootCategory);
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
					RewindCategory();
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
			CurrentCategory = category;
			RewindCategory();
		}
		
		private bool PopCategory()
		{
			if (_navigationStack.Count <= 1) //always keep root category on the stack
			{
				return false;
			}

			_navigationStack.Pop();
			CurrentCategory = _navigationStack.Peek();
			RewindCategory();
		}

		private void RewindCategory()
		{
			_collectionOffset = 0;
			RefreshDisplayList();
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

			lock (_readingMutex)
			{
				if (_isReading)
				{
					return;
				}
			}

			_correctionCategory.Children.Clear();
			foreach (IndiagramUIModel indiagramUI in SentenceIndiagrams)
			{
				_correctionCategory.Children.Add(indiagramUI.Model);
			}
			
			PushCategory(_correctionCategory);
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
			lock (_readingMutex)
			{
				if (_isReading)
				{
					return;
				}
			}

			Read(indiagram);
			Category category = indiagram as Category;
			if (category != null)
			{
				PushCategory(category);
				return;
			}


		}

		private void Read(Indiagram indiagram)
		{
			if (indiagram.IsCategory)
			{
				if (SettingsService.IsCategoryNameReadingEnabled)
				{
					
				}
			}
		}

		#region Read sentence handling part

		private void ReadSentenceAction()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
