﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Storage.Sqlite.Model;
using SQLite.Net;
using SQLite.Net.Interop;
using Storm.Mvvm.Extensions;
using Storm.Mvvm.Inject;

namespace IndiaRose.Storage.Sqlite
{
	public class SqliteCollectionStorageService : ICollectionStorageService
	{
		public event EventHandler Initialized;

		//Attributes 
		private readonly ISQLitePlatform _platform;
		private SQLiteConnection _connection;

		private List<IndiagramSql> _databaseContent;
		private readonly ObservableCollection<Indiagram> _collection = new ObservableCollection<Indiagram>();

		private bool _isInitialized = false;

		public ObservableCollection<Indiagram> Collection
		{
			get { return _collection; }
		}

		public bool IsInitialized
		{
			get { return _isInitialized; }
			private set
			{
				if (!_isInitialized && value)
				{
					_isInitialized = true;

					EventHandler handler = Initialized;
					if (handler != null)
					{
						handler(this, EventArgs.Empty);
					}
				}
			}
		}

		protected SQLiteConnection Connection
		{
			get { return _connection; }
		}

		public SqliteCollectionStorageService(ISQLitePlatform platform)
		{
			_platform = platform;
		}

		public async Task InitializeAsync()
		{
			string dbPath = LazyResolver<IStorageService>.Service.DatabasePath;

			// Initialize connection
			_connection = new SQLiteConnection(_platform, dbPath);
			_connection.CreateTable<IndiagramSql>();

			// Load all the database
			_databaseContent = Connection.Table<IndiagramSql>().OrderBy(x => x.ParentId).ThenBy(x => x.Position).ToList();

			// Load the collection
			Category collectionRoot = new Category { Id = IndiagramSql.ROOT_PARENT };
			List<Category> categories = new List<Category> { collectionRoot };

			for (int i = 0; i < categories.Count; ++i)
			{
				Category category = categories[i];

				_databaseContent.SkipWhile(x => x.ParentId != category.Id).TakeWhile(x => x.ParentId == category.Id).ForEach(x =>
				{
					Indiagram indiagram = x.ToModel();
					indiagram.Parent = category;
					category.Children.Add(indiagram);
					if (indiagram.IsCategory)
					{
						categories.Add(indiagram as Category);
					}
				});
			}
            collectionRoot.Children.ForEach(x =>
            {
                x.Parent = null;
                Collection.Add(x);
            });

			_databaseContent = _databaseContent.OrderBy(x => x.Id).ToList();

			IsInitialized = true;
		}

		public Indiagram Save(Indiagram indiagram)
		{
			if (indiagram == null)
			{
				throw new ArgumentNullException("indiagram");
			}

			if (indiagram.Id > 0)
			{
				return Update(indiagram);
			}
			return Create(indiagram);
		}

		private Indiagram Update(Indiagram indiagram)
		{
			IndiagramSql sqlObject = SearchById(indiagram.Id);
			if (sqlObject == null)
			{
				throw new InvalidOperationException(string.Format("Can not update a non created object, id = {0}", indiagram.Id));
			}

			sqlObject.FromModel(indiagram);
			Connection.Update(sqlObject);

			return indiagram;
		}

		private Indiagram Create(Indiagram indiagram)
		{
			IndiagramSql sqlObject = new IndiagramSql();
			sqlObject.FromModel(indiagram);

			Connection.Insert(sqlObject);
			indiagram.Id = sqlObject.Id;
			_databaseContent.Add(sqlObject);
			return indiagram;
		}

		public void Delete(Indiagram indiagram)
		{
			// remove from parent tree or from home root category
			Category parent = indiagram.Parent as Category;
			if (parent != null)
			{
				parent.Children.Remove(indiagram);
				Save(parent);
			}
			else
			{
				_collection.Remove(indiagram);
			}

			// Delete children if have any
			if (indiagram.IsCategory)
			{
				DeleteTree(indiagram as Category);
			}

			// Delete this object
			IndiagramSql sqlObject = SearchById(indiagram.Id);
			_databaseContent.Remove(sqlObject);
			Connection.Delete<IndiagramSql>(indiagram.Id);
		}

		#region Private helpers methods

		private void DeleteTree(Category category)
		{
			if (category == null)
			{
				return;
			}

			category.Children.ForEach(x =>
			{
				IndiagramSql indiagram = SearchById(x.Id);

				if (x.IsCategory)
				{
					DeleteTree(x as Category);
				}

				_databaseContent.Remove(indiagram);
				Connection.Delete<IndiagramSql>(indiagram.Id);
			});
		}

		/// <summary>
		/// Look into the database by id to find an indiagram
		/// </summary>
		/// <param name="id">The id of the indiagram</param>
		/// <returns>The indiagram which is in the database</returns>
		private IndiagramSql SearchById(int id)
		{
			int start = 0;
			int end = _databaseContent.Count;

			while (true)
			{
				int currentMidIndex = (end + start)/2;
				IndiagramSql currentMidValue = _databaseContent[currentMidIndex];

				if (currentMidValue.Id == id)
				{
					return currentMidValue;
				}
				if (id > currentMidValue.Id)
				{
					start = currentMidIndex + 1;
				}
				else
				{
					end = currentMidIndex - 1;
				}

				if (start > end)
				{
					return null;
				}
			}
		}

		#endregion
	}
}

