using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Storage.Sqlite.Model;

using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace IndiaRose.Storage.Sqlite
{
	public class SqliteCollectionStorageService : ICollectionStorageService
	{
		private const string DB_PATH = "india.sqlite";
		private readonly ISQLitePlatform _platform;
		private SQLiteConnection _connection;

		protected SQLiteConnection Connection
		{
			get { return _connection ?? (_connection = OpenDatabase()); }
		}

		public SqliteCollectionStorageService(ISQLitePlatform platform)
		{
			_platform = platform;
		}

		private SQLiteConnection OpenDatabase()
		{
			SQLiteConnection database = new SQLiteConnection(_platform, DB_PATH);

			//TODO : create table ?

			return database;
		}

	    public void Create(Indiagram indiagram)
	    {
	        if (indiagram is Category)
	        {
	            var temp = new CategorySql
	            {
	                ImagePath = indiagram.ImagePath,
	                SoundPath = indiagram.SoundPath,
	                Text = indiagram.Text
	            };
	            temp.Position = temp.Id;
	            temp.Parent = (GetIndiagramSql(indiagram.Parent)).Id;
	        }
	        else
	        {
	            var temp = new IndiagramSql
	            {
	                ImagePath = indiagram.ImagePath,
	                SoundPath = indiagram.SoundPath,
	                Text = indiagram.Text
	            };
	            temp.Position = temp.Id;
                //TODO add avec connexion
	        }
	    }

        public void ChangeCategory(Indiagram indiagram, Category category)
        {
            throw new NotImplementedException();

            indiagram.Parent = category;
            Update(indiagram);
        }

        public void Update(Indiagram indiagram)
        {
            throw new NotImplementedException();

            //TODO connexion impossible
            /*var db = new SQLiteConnection(dbPath);

            if (indiagram is Category)
            {
                var query = db.Table<CategorySql>().SingleOrDefault(t => t.Text == indiagram.Text ||
                    t.ImagePath == indiagram.ImagePath || t.SoundPath == indiagram.SoundPath ||
                    t.Position == indiagram.Position ||
                    t.Position == indiagram.Position);
                query.Text = indiagram.Text;
                query.ImagePath = indiagram.ImagePath;
                query.SoundPath = indiagram.SoundPath;
                //query.Children.CopyTo(indiagram.Children);
                db.Update(query);
            }
            else
            {
                var query = db.Table<IndiagramSql>().SingleOrDefault(t => t.Text == indiagram.Text ||
                    t.ImagePath == indiagram.ImagePath || t.SoundPath == indiagram.SoundPath ||
                    t.Position == indiagram.Position || t.Position == indiagram.Position);
                query.Text = indiagram.Text;
                query.ImagePath = indiagram.ImagePath;
                query.SoundPath = indiagram.SoundPath;
                db.Update(query);
            }
            db.Close();*/
        }

		public void Delete(Indiagram indiagram)
		{

            throw new NotImplementedException();

            //connection
		    //var db = new SQLiteConnection();
		    //db.Delete<IndiagramSql>(GetIndiagramSql(indiagram));
		}

	    private IndiagramSql GetIndiagramSql(Indiagram indiagram)
	    {
            throw new NotImplementedException();
	        /*IndiagramSql tsql;
	        foreach (var t in GetFullCollection())
	        {
	            tsql =
	            if((t.ImagePath.Equals(indiagram.ImagePath))&&(t.Position==indiagram.Position)&&(t.SoundPath.Equals(indiagram.SoundPath)))
	            {
	                return tsql;
	            }
             * return null;
	        }*/
	    }

	    private Indiagram GetIndiagramFromSql(IndiagramSql indiagram)
	    {

            throw new NotImplementedException();
            /*var temp = from s in db.Table<Indiagram>()
                        where
            * 
            * Return same position <-->
            * */
	    }

	    private Indiagram SearchCategory(IndiagramSql csql)
	    {
	        if (csql is CategorySql)
	        {
	            Category c = new Category()
	            {
	                Text = csql.Text,
	                ImagePath = csql.ImagePath,
	                SoundPath = csql.SoundPath,
	                Position = csql.Position,
	                //Children = {AddCategory(((CategorySql) csql).Children)},
                    //{ if (csql.Position != 0) Position = csql.Position }
	            };

	            return c;
	        }
	        else
	        {
	            Indiagram i = new Indiagram()
	            {
	                Text = csql.Text,
	                ImagePath = csql.ImagePath,
	                SoundPath = csql.SoundPath,
	                Position = csql.Position,
                    //{ if (csql.Position != 0) Position = csql.Position }
	            };

	            return i;
	        }
	    }

        private List<Indiagram> AddCategory(CategorySql csql)
        {
            Indiagram i;
            List<Indiagram> list = new List<Indiagram>();

            foreach (var c in csql.Children)
            {
                list.Add(SearchCategory(c));
            }

            return list;
        }

        public List<Indiagram> GetTopLevel()
        {
            throw new NotImplementedException();

            //TODO connexion impossible
            /*Indiagram indiagram;
            List<Indiagram> list = new List<Indiagram>();

            var db = new SQLiteConnection(dbPath);
            var table = db.Table<CategorySql>();
            var table2 = db.Table<IndiagramSql>();

            foreach (var v in table)
            {
                indiagram = SearchCategory(v);

                if (indiagram.Parent != null)
                {
                    list.Add(SearchCategory(v));
                }
            }

            foreach (var i in table2)
            {
                if (i.Parent != 0)
                {
                    indiagram = new Indiagram()
                    {
                        Text = i.Text,
                        ImagePath = i.ImagePath,
                        SoundPath = i.SoundPath,
                        Position = i.Position,
                        Parent = null
                    };

                    list.Add(indiagram);
                }
            }

            return list;*/
        }

	    private List<Indiagram> SearchChildren(List<Indiagram> topLevel, Indiagram parent)
	    {
	        foreach (var table in topLevel)
	        {
	            if (table is Category)
	            {
	                if (table.Equals(parent))
	                {
	                    return table.Children;
	                }
	                return SearchChildren(table.Children, parent);
	            } 
	        }

	        return null;
	    }

        public List<Indiagram> GetChildren(Indiagram parent)
        {
            throw new NotImplementedException();

            //TODO connexion impossible

            List<Indiagram> list = GetTopLevel();

            return SearchChildren(list, parent);
        }

		public List<Indiagram> GetFullCollection()
		{
            throw new NotImplementedException();

		    var list = new List<Indiagram>();
		    foreach (var t in GetTopLevel())
		    {
		        list = AddChildren(list, t);
		    }
		    return list;

		}

	    private List<Indiagram> AddChildren(List<Indiagram> list, Indiagram indiagram)
	    {
	        if (!(indiagram is Category)) return list;
	        foreach (var t in indiagram.Children)
	        {
	            list.Add(t);
	            if (t is Category)
	            {
	                return AddChildren(list, t);
	            }
	        }
	        return list;
	    }
	}
}
