using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Storage;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels
{
	public abstract class AbstractBrowserViewModel : AbstractViewModel
	{
		private int _collectionOffset;
		private int _collectionDisplayCount;
		private List<Indiagram> _displayedIndiagrams;

		private Category _rootCollection;
		private readonly Stack<Category> _navigationStack = new Stack<Category>();

		#region Services

		protected IMessageDialogService MessageDialogService
		{
			get { return LazyResolver<IMessageDialogService>.Service; }
		}

		protected ISettingsService SettingsService
		{
			get { return LazyResolver<ISettingsService>.Service; }
		}

		protected ICollectionStorageService CollectionStorageService
		{
			get { return LazyResolver<ICollectionStorageService>.Service; }
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

		#endregion

		#region Commands

		public ICommand NextCommand { get; private set; }

		public ICommand IndiagramSelectedCommand { get; private set; }

		#endregion

		protected AbstractBrowserViewModel()
		{
			NextCommand = new DelegateCommand(NextAction);
			IndiagramSelectedCommand = new DelegateCommand<Indiagram>(IndiagramSelectedAction);

			// Load collection
			LoadCollection();
		}

		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			base.OnNavigatedTo(e, parametersKey);

			if (_navigationStack.Any())
			{
				RefreshDisplayList();
			}
			else
			{
				PushCategory(_rootCollection);
			}
		}

		protected void LoadCollection()
		{
			List<Indiagram> collection = CollectionStorageService.GetTopLevel();

			// debug purpose only
			if (collection.Count == 0)
			{
				CollectionStorageService.Create(new Indiagram("azerty", ""));
				CollectionStorageService.Create(new Indiagram("helloa", ""));
				CollectionStorageService.Create(new Indiagram("helloz", ""));
				CollectionStorageService.Create(new Indiagram("helloe", ""));
				CollectionStorageService.Create(new Indiagram("hellor", ""));
				CollectionStorageService.Create(new Indiagram("hellot", ""));
				CollectionStorageService.Create(new Indiagram("helloy", ""));
				CollectionStorageService.Create(new Indiagram("hellou", ""));
				CollectionStorageService.Create(new Indiagram("helloi", ""));
				CollectionStorageService.Create(new Indiagram("helloo", ""));
				CollectionStorageService.Create(new Indiagram("hellop", ""));
				CollectionStorageService.Create(new Indiagram("helloq", ""));
				CollectionStorageService.Create(new Indiagram("hellos", ""));
				CollectionStorageService.Create(new Indiagram("hellod", ""));
				CollectionStorageService.Create(new Indiagram("hellof", ""));
				CollectionStorageService.Create(new Indiagram("hellog", ""));
				CollectionStorageService.Create(new Indiagram("helloh", ""));
				CollectionStorageService.Create(new Indiagram("helloj", ""));
				CollectionStorageService.Create(new Indiagram("hellok", ""));
				CollectionStorageService.Create(new Indiagram("hellol", ""));
				CollectionStorageService.Create(new Indiagram("hellom", ""));
				CollectionStorageService.Create(new Indiagram("hellow", ""));

				collection = CollectionStorageService.GetTopLevel();
			}

			_rootCollection = new Category("Home", "");
			_rootCollection.Children.AddRange(collection);
		}

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

		protected void PushCategory(Category category)
		{
			_navigationStack.Push(category);
			CollectionOffset = 0;
			DisplayedIndiagrams = FilterCollection(category.Children).ToList();
		}

		protected void PopCategory()
		{
			if (_navigationStack.Count <= 1)
			{
				return;
			}

			_navigationStack.Pop();
			CollectionOffset = 0;
			RefreshDisplayList();
		}

		protected void RewindCategory()
		{
			CollectionOffset = 0;
		}

		protected void RefreshDisplayList()
		{
			if (_navigationStack.Count == 0)
			{
				return;
			}

			DisplayedIndiagrams = FilterCollection(_navigationStack.Peek().Children).ToList();
		}

		/// <summary>
		/// Override this function to be able to display only part of the collection
		/// </summary>
		/// <param name="input">The full indiagram list</param>
		/// <returns>The part of the list you want to display (for instance, only category)</returns>
		protected virtual IEnumerable<Indiagram> FilterCollection(List<Indiagram> input)
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
