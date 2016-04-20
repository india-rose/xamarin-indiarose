using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels
{
    /// <summary>
    /// VueModèle abstrait pour les pages utilisant l'affichage de la collection
    /// </summary>
	public abstract class AbstractBrowserViewModel : AbstractViewModel
    {
		private int _collectionOffset;
		private int _collectionDisplayCount;
		private List<Indiagram> _displayedIndiagrams;
		// only used when correction mode enabled
		private bool _canCategoryBePopped = true;

		private readonly Category _rootCollection;
		private readonly Stack<Category> _navigationStack = new Stack<Category>();

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

		#region Public properties


		public string TextColor
		{
			get { return SettingsService.TextColor; }
		}

		public string BackgroundColor
		{
			get { return SettingsService.TopBackgroundColor; }
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

		public List<Indiagram> DisplayedIndiagrams
		{
			get { return _displayedIndiagrams; }
			private set { SetProperty(ref _displayedIndiagrams, value); }
		}

	    public Category CurrentCategory
	    {
            get { return _navigationStack != null && _navigationStack.Any() ? _navigationStack.Peek() : null; }
	    }
		#endregion

		#region Commands

		public ICommand NextCommand { get; private set; }

        public ICommand BackCategoryCommand { get; private set; }

		public ICommand IndiagramSelectedCommand { get; private set; }

		#endregion

		protected AbstractBrowserViewModel()
		{
			NextCommand = new DelegateCommand(NextAction);
            BackCategoryCommand = new DelegateCommand(BackCategoryAction);
			IndiagramSelectedCommand = new DelegateCommand<Indiagram>(IndiagramSelectedAction);
            
            // Load collection
            ObservableCollection<Indiagram> collection = CollectionStorageService.Collection;

            //Création de la catégorie Home
			_rootCollection = new Category(collection)
			{
                Text = LocalizationService.GetString("Collection_RootCategoryName", "Text"),
                ImagePath = StorageService.ImageRootPath,
            };
        }

        /// <summary>
        /// Méthode s'exécutant lorsqu'on arrive sur la pge
        /// On push la catégorie Home sur le sommet de la pile
        /// </summary>
		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
            //todo debug windows line
            if (e != null) 
			base.OnNavigatedTo(e, parametersKey);

			if (!_navigationStack.Any())
			{
				PushCategory(_rootCollection);
			}
		}

        /// <summary>
        /// Action lorsqu'on appuie sur le bouton Next de la collection
        /// Recalcule les offset
        /// </summary>
		private void NextAction()
		{
			int offset = CollectionOffset;
			offset += CollectionDisplayCount;

			if (offset >= DisplayedIndiagrams.Count)
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

        /// <summary>
        /// Action lorsqu'on appuie sur le bouton BackCategory de la collection
        /// Reviens sur la categorie précédente
        /// </summary>
		private void BackCategoryAction()
        {
                if (_navigationStack.Count > 1)
                {
                    PopCategory();
                }
        }

        /// <summary>
        /// Ajoute une nouvelle catégorie sur le sommet de la pile de navigation
        /// </summary>
        /// <param name="category">Categorie à push</param>
        /// <param name="canBePopped">Indique si la nouvelle catégorie peut être pop</param>
		protected void PushCategory(Category category, bool canBePopped = true)
		{
			if (_navigationStack.Any())
			{
				_navigationStack.Peek().Children.CollectionChanged -= OnCollectionChanged;
			}

			_canCategoryBePopped = canBePopped;
			category.Children.CollectionChanged += OnCollectionChanged;

			_navigationStack.Push(category);
            RaisePropertyChanged("CurrentCategory");
			RewindCategory();
			RefreshDisplayList();
		}

        /// <summary>
        /// Callback raffraichissant la liste
        /// </summary>
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			RefreshDisplayList();
		}

        /// <summary>
        /// Retourne à la catégorie précédente
        /// </summary>
        /// <param name="force">Force le pop même si la catégorie ne peut pas être pop (par défaut à faux)</param>
        /// <returns>Retourne vrai si on a pop, faux sinon</returns>
		protected bool PopCategory(bool force = false)
		{
			if (_navigationStack.Count <= 1)
			{
			    return false;
			}

			if (!_canCategoryBePopped)
			{
				if (force)
				{
					_canCategoryBePopped = true;
				}
				else
				{
					return false;
				}
			}

			_navigationStack.Pop().Children.CollectionChanged -= OnCollectionChanged;
            _navigationStack.Peek().Children.CollectionChanged += OnCollectionChanged;
            RaisePropertyChanged("CurrentCategory");
			RewindCategory();
			RefreshDisplayList();
		    return true;
		}

		protected void RewindCategory()
		{
			CollectionOffset = 0;
		}

        /// <summary>
        /// Raffraichit l'affichage de la liste
        /// </summary>
		protected void RefreshDisplayList()
		{
			if (_navigationStack.Count == 0)
			{
				return;
			}

			int lastCollectionCount = CollectionDisplayCount;
			if (lastCollectionCount <= 0)
			{
				// TODO: See to be able to adapt this value automatically
				lastCollectionCount = 12;
			}
			DisplayedIndiagrams = FilterCollection(_navigationStack.Peek().Children).ToList();

            //Si on est en bout de liste on revient à la catégorie précédente
			if (DisplayedIndiagrams.Count == 0 && _navigationStack.Count > 1)
			{
				PopCategory();
			}

			if (CollectionOffset >= DisplayedIndiagrams.Count)
			{
				CollectionOffset = Math.Max(0, CollectionOffset - lastCollectionCount);
			}
		}

		/// <summary>
		/// Override this function to be able to display only part of the collection
		/// </summary>
		/// <param name="input">The full indiagram list</param>
		/// <returns>The part of the list you want to display (for instance, only category)</returns>
		protected virtual IEnumerable<Indiagram> FilterCollection(IEnumerable<Indiagram> input)
		{
			return input;
		}

		/// <summary>
		/// Override this function to receive callback when an indiagram is selected in the browser
		/// </summary>
		/// <param name="indiagram"></param>
		protected virtual void IndiagramSelectedAction(Indiagram indiagram)
		{
		    // Do something with this indiagram
		}
	}
}
