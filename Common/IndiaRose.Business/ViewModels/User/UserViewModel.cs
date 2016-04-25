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
    /// <summary>
    /// VueModèle de la page Utilisateur
    /// </summary>
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
        private Category _parentCategory;
		private bool _isReading;
		private bool _isCorrectionModeEnabled;
		private bool _sentenceCanAddMoreIndiagrams = true;
		private int _sentenceIndiagramId = -242;

        #region Services

        public ISettingsService SettingsService => LazyResolver<ISettingsService>.Service;

        protected ICollectionStorageService CollectionStorageService => LazyResolver<ICollectionStorageService>.Service;

        protected IStorageService StorageService => LazyResolver<IStorageService>.Service;

        protected ITextToSpeechService TextToSpeechService => LazyResolver<ITextToSpeechService>.Service;

        #endregion

        #region Properties

        /// <summary>
        /// Catégorie Courante
        /// </summary>
		public Category CurrentCategory
		{
			get { return _currentCategory; }
			set
			{
				if (SetProperty(ref _currentCategory, value) && value != null)
				{
                    //si on change de catégorie on raffraichit l'affichage
					RefreshDisplayList();
				}
			}
		}

        public Category ParentCategory
        {
            get { return _parentCategory; }
            set
            {
                //if (_navigationStack.Count != 1)
                    SetProperty(ref _parentCategory, value);
            }
        }

        /// <summary>
        /// Collection d'Indiagram à afficher
        /// </summary>
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

        /// <summary>
        /// La liste des Indiagrams dans la phrase
        /// </summary>
		public ObservableCollection<IndiagramUIModel> SentenceIndiagrams => _sentenceIndiagrams;

        /// <summary>
        /// Booléen indiquant si on peut encore ajouter des Indiagrams à la phrase
        /// S'il est à faux, en cas d'ajout il ne se passera rien
        /// </summary>
		public bool SentenceCanAddMoreIndiagrams
		{
			get { return _sentenceCanAddMoreIndiagrams; }
			set { SetProperty(ref _sentenceCanAddMoreIndiagrams, value); }
		}

        /// <summary>
        /// Vrai si on est dans le mode Correction
        /// </summary>
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
        public ICommand CollectionBackCategoryCommand { get; private set; }
		public ICommand ReadSentenceCommand { get; private set; }
		public ICommand CollectionIndiagramDragStartCommand { get; private set; }

        #endregion

		public UserViewModel()
		{
            //Création des catégories spéciales (Home et Correction)
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

            CollectionBackCategoryCommand = new DelegateCommand(CollectionBackCategoryAction);

            ReadSentenceCommand = new DelegateCommand(ReadSentenceAction);

            PushCategory(rootCategory);

            TextToSpeechService.SpeakingCompleted += OnIndiagramReadCompleted;
        }

        #region Collection navigation

        /// <summary>
        /// Action résultant de l'appuie sur le bouton next de la collection
        /// Affiche les Indiagrams suivants ou revient à la catégorie précédente
        /// </summary>
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
        /// <summary>
        /// Action résultant de l'appuie sur le bouton back de la collection
        /// Revient à la catégorie précédente
        /// </summary>
        private void CollectionBackCategoryAction()
        {
            if (_navigationStack.Count > 1)
            {
                PopCategory();
            }
        }

        /// <summary>
        /// Push une catégorie sur la pile de navigation, reset l'offset d'affichage et change la catégorie courante
        /// </summary>
		private void PushCategory(Category category)
		{
			_navigationStack.Push(category);
			CollectionOffset = 0;
            ParentCategory = CurrentCategory;
            CurrentCategory = category;
		}
		
        /// <summary>
        /// Pop une catégorie de la stack, reset l'offset d'affichage et change la catégorie courante
        /// </summary>
        /// <returns>Retourne faux si la catégorie courante est la Home</returns>
		private bool PopCategory()
		{
			if (_navigationStack.Count <= 1) //always keep root category on the stack
			{
				return false;
			}
            _navigationStack.Pop();
			CollectionOffset = 0;
            if (_navigationStack.Count == 1)
                ParentCategory = new Category();
            else
                ParentCategory = _navigationStack.ElementAt(_navigationStack.Count - 1);
            CurrentCategory = _navigationStack.Peek();

            return true;
        }

        /// <summary>
        /// Change la collection d'Indiagram à afficher
        /// </summary>
		private void RefreshDisplayList()
		{
			if (_navigationStack.Count == 0)
			{
				return;
			}

            CollectionIndiagrams = CurrentCategory.Children.Where(
                SettingsService.IsMultipleIndiagramSelectionEnabled && !IsCorrectionModeEnabled ?
                    (Func<Indiagram, bool>)(x => x.IsEnabled) :
                    (x => x.IsEnabled && SentenceIndiagrams.All(y => y.Model.Id != x.Id))).ToList();
        }

        #endregion

        /// <summary>
        /// Entre dans le mode Correction
        /// </summary>
		private void EnterCorrectionModeAction()
		{
			if (SentenceIndiagrams.Count == 0 || CheckIsReading() || IsCorrectionModeEnabled)
			{
				return;
			}

            _correctionCategory.Children.Clear();
            foreach (IndiagramUIModel indiagramUI in SentenceIndiagrams)
            {
                _correctionCategory.Children.Add(indiagramUI.Model);
            }

            SentenceIndiagrams.Clear();
            IsCorrectionModeEnabled = true;
            PushCategory(_correctionCategory);
        }

        /// <summary>
        /// Action lorsqu'un Indiagram dans la phrase est selectionné
        /// Retire l'Indiagram de la phrase et le rajoute dans la collection
        /// </summary>
        /// <param name="indiagram"></param>
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
            if (!SettingsService.IsMultipleIndiagramSelectionEnabled || IsCorrectionModeEnabled)
            {
                RefreshDisplayList();
            }
        }

        /// <summary>
        /// Action lors de la sélection d'un Indiagram de la partie Collection
        /// </summary>
        /// <param name="indiagram">L'Indiagram sélectionné</param>
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

                    if (SettingsService.IsBackHomeAfterSelectionEnabled && !IsCorrectionModeEnabled && PopCategory())
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

        /// <summary>
        /// Action lorsqu'un drag & drop est activé de la partie Collection
        /// </summary>
        /// <param name="indiagram">L'Indiagram sélectionné</param>
		private void CollectionIndiagramDragStartAction(Indiagram indiagram)
		{
			if (CheckIsReading())
			{
				return;
			}

            Read(indiagram);
        }

        /// <summary>
        /// Lance la lecture d'un Indiagram par le TTS
        /// </summary>
        /// <param name="indiagram">Indiagram à lire</param>
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

        /// <summary>
        /// Callback lors de la fin de la lecture d'un Indiagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
		private void OnIndiagramReadCompleted(object sender, EventArgs eventArgs)
		{
			lock (_readingMutex)
			{
				_isReading = false;
			}
		}


        #region Read sentence handling part

        /// <summary>
        /// Ajoute un Indiagram à la phrase à lire (partie du bas)
        /// Ne check pas s'il y a de la place pour ajouter, à faire avant d'appeler cette méthode
        /// </summary>
        /// <param name="indiagram">L'Indiagram à ajouté</param>
		private void AddIndiagramToSentence(Indiagram indiagram)
		{
			if (SettingsService.IsMultipleIndiagramSelectionEnabled && !IsCorrectionModeEnabled)
			{
				// need to create a copy of the indiagram
				Indiagram copy = new Indiagram();
				copy.CopyFrom(indiagram);
				indiagram = copy;
				indiagram.Id = _sentenceIndiagramId--;
			}

            SentenceIndiagrams.Add(new IndiagramUIModel(indiagram));
        }

        /// <summary>
        /// Action pour lancer la lecture de la phrase d'Indiagram
        /// </summary>
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

        /// <summary>
        /// Lit la phrase d'Indiagram
        /// Méthode asynchrone
        /// </summary>
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
                //AVANT
                _sentenceReadingSemaphore.WaitOne();
                //APRES
                // wait delay specified in settings before going to next one
                int millisecondsToWait = (int)(SettingsService.TimeOfSilenceBetweenWords * 1000);
                if (millisecondsToWait > 10)
                {
                    await Task.Delay(millisecondsToWait);
                }

                // disable reinforcer
                if (isReinforcerEnabled)
                {
                    DispatcherService.InvokeOnUIThread(() => currentIndiagram.IsReinforcerEnabled = false);
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

        /// <summary>
        /// Callback de la fin de lecture d'une phrase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
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
