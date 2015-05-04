using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Storage.Sqlite.Model;
using SQLite.Net;
using SQLite.Net.Interop;
using Storm.Mvvm.Extensions;
using Storm.Mvvm.Inject;

namespace IndiaRose.Storage.Sqlite
{
	public class SqliteCollectionStorageService : ICollectionStorageService
	{
		//Attributes 
		private readonly SQLiteConnection _connection;

		private readonly List<IndiagramSql> _databaseContent;
		private readonly ObservableCollection<Indiagram> _collection;

		public ObservableCollection<Indiagram> Collection
		{
			get { return _collection; }
		}

		protected SQLiteConnection Connection
		{
			get { return _connection; }
		}

		public SqliteCollectionStorageService(ISQLitePlatform platform)
		{
			string dbPath = LazyResolver<IStorageService>.Service.DatabasePath;

			// Initialize connection
			_connection = new SQLiteConnection(platform, dbPath);
			_connection.CreateTable<IndiagramSql>();

			// Load all the database
			_databaseContent = Connection.Table<IndiagramSql>().OrderBy(x => x.Position).ToList();
			
			// Load the collection
			Category collectionRoot = new Category {Id = IndiagramSql.ROOT_PARENT};
			List<Indiagram> categories = new List<Indiagram> { collectionRoot };

			for (int i = 0; i < categories.Count; ++i)
			{
				Category category = categories[i] as Category;
				if (category != null)
				{
					_databaseContent.FindAll(x => x.ParentId == category.Id).Select(x => x.ToModel()).ForEach(x => category.Children.Add(x));
					category.Children.ForEach(x => x.Parent = category);
					categories.AddRange(category.Children.Where(x => x.IsCategory));
				}
			}
			_collection = new ObservableCollection<Indiagram>(collectionRoot.Children);
			Collection.ForEach(x => x.Parent = null);
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
			else
			{
				return Create(indiagram);
			}
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

			IndiagramSql sqlObject = SearchById(indiagram.Id);
			_databaseContent.Remove(sqlObject);
			Connection.Delete<IndiagramSql>(indiagram.Id);
		}

		#region Private helpers methods

		/// <summary>
		/// Look into the database by id to find an indiagram
		/// </summary>
		/// <param name="id">The id of the indiagram</param>
		/// <returns>The indiagram which is in the database</returns>
		private IndiagramSql SearchById(int id)
		{
			return _databaseContent.SingleOrDefault(x => x.Id == id);
		}

		#endregion
	}
}

