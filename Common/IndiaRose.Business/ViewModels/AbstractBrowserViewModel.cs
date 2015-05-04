﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

		private readonly Category _rootCollection;
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
			ObservableCollection<Indiagram> collection = CollectionStorageService.Collection;

			// debug purpose only
			if (collection.Count == 0)
			{
				int position = 1;
				Func<string, Indiagram> constructorLambda = text => new Indiagram
				{
					Text = text,
					ImagePath = "",
					Position = position++,
				};
				collection.Add(CollectionStorageService.Save(constructorLambda("azerty")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloa")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloz")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloe")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellor")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellot")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloy")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellou")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloi")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloo")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellop")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloq")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellos")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellod")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellof")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellog")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloh")));
				collection.Add(CollectionStorageService.Save(constructorLambda("helloj")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellok")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellol")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellom")));
				collection.Add(CollectionStorageService.Save(constructorLambda("hellow")));
			}

			//TODO: translate "home" and add an image
			_rootCollection = new Category(collection)
			{
				Text = "Home",
				ImagePath = "",
			};
		}

		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			base.OnNavigatedTo(e, parametersKey);

			if (!_navigationStack.Any())
			{
				PushCategory(_rootCollection);
			}
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
			if (_navigationStack.Any())
			{
				_navigationStack.Peek().Children.CollectionChanged -= OnCollectionChanged;
			}

			category.Children.CollectionChanged += OnCollectionChanged;

			_navigationStack.Push(category);
			RewindCategory();
			RefreshDisplayList();
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			RefreshDisplayList();
		}

		protected void PopCategory()
		{
			if (_navigationStack.Count <= 1)
			{
				return;
			}

			_navigationStack.Pop().Children.CollectionChanged -= OnCollectionChanged;
			_navigationStack.Peek().Children.CollectionChanged += OnCollectionChanged;
			RewindCategory();
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

			int lastCollectionCount = CollectionDisplayCount;
			if (lastCollectionCount <= 0)
			{
				// TODO: See to be able to adapt this value automatically
				lastCollectionCount = 12;
			}
			DisplayedIndiagrams = FilterCollection(_navigationStack.Peek().Children).ToList();

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
