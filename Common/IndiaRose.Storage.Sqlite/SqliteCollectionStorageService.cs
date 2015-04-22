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
            var database = new SQLiteConnection(_platform, DB_PATH);
            database.CreateTable<IndiagramSql>();
            database.CreateTable<CategorySql>();
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
                Connection.Insert(temp);
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
                Connection.Insert(temp);
            }
        }

        public void ChangeCategory(Indiagram indiagram, Category category)
        {
            indiagram.Parent = category;
            Update(indiagram);
        }

        public void Update(Indiagram indiagram)
        {
            //Connection.Update(indiagram);
            /**
             * 
             * a tester
             * 
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
            var current = GetIndiagramSql(indiagram);
            if (current is CategorySql)
            {
                Connection.Delete<CategorySql>(current.Id);
            }
            else
            {
                Connection.Delete<IndiagramSql>(current.Id);
            }
        }

        private IndiagramSql GetIndiagramSql(Indiagram indiagram)
        {
            if (indiagram is Category)
            {
                return Connection.Table<CategorySql>().SingleOrDefault(t => t.Text == indiagram.Text &&
                                                                            t.ImagePath == indiagram.ImagePath &&
                                                                            t.SoundPath == indiagram.SoundPath &&
                                                                            t.Position == indiagram.Position &&
                                                                            t.Position == indiagram.Position);
            }

            return Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Text == indiagram.Text &&
                                                                         t.ImagePath == indiagram.ImagePath &&
                                                                         t.SoundPath == indiagram.SoundPath &&
                                                                         t.Position == indiagram.Position &&
                                                                         t.Position == indiagram.Position);
        }

        private Indiagram GetIndiagramFromSql(IndiagramSql indiagram)
        {
            throw new NotImplementedException();
            if (indiagram is CategorySql)
            {
                var csql = (CategorySql) indiagram;
                var c = new Category()
                {
                    Text = csql.Text,
                    ImagePath = csql.ImagePath,
                    SoundPath = csql.SoundPath,
                    Position = csql.Position,
                };
                if ((csql.Children) == null)
                    return c;

            }
            var i = new Indiagram
            {
                Text = indiagram.Text,
                ImagePath = indiagram.ImagePath,
                SoundPath = indiagram.SoundPath,
                Position = indiagram.Position,
                Parent = GetIndiagramFromSql(SearchByIdSql(indiagram.Parent)),
            };

            return i;
        }

        private IndiagramSql SearchByIdSql(int id)
        {
            return Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Id == id);
        }


    private List<Indiagram> GetChildren(Category category)
	    {
	       return category.Children;
	    }

        private List<Indiagram> AddCategory(CategorySql csql)
        {
            Indiagram i;
            List<Indiagram> list = new List<Indiagram>();

            foreach (var c in csql.Children)
            {
                list.Add(GetIndiagramFromSql(c));
            }

            return list;
        }

        public List<Indiagram> GetTopLevel()
        {
            Indiagram indiagram;
            var list = new List<Indiagram>();
            var table = Connection.Table<CategorySql>();
            var table2 = Connection.Table<IndiagramSql>();

            foreach (var v in table)
            {
                indiagram = GetIndiagramFromSql(v);

                if (indiagram.Parent != null)
                {
                    list.Add(indiagram);
                }
            }

            foreach (var i in table2.Where(i => i.Parent != 0))
            {
                indiagram = GetIndiagramFromSql(i);

                list.Add(indiagram);
            }

            return list;
        }

        //TODO 
        private Indiagram SearchCategory(CategorySql p0)
        {
            throw new NotImplementedException();
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
		    var list = new List<Indiagram>();
            //Parcourir le topLevel dans un premier temps
		    foreach (var t in GetTopLevel())
		    {
                list = AddChildren(list, t);/*ajouter les enfants avec la methode recursive
                                             *(1er appel)
                                             * 
                                             * 
                                             */
		    }
		    return list;

		}

	    private List<Indiagram> AddChildren(List<Indiagram> list, Indiagram indiagram)
	    {
	        if (!(indiagram is Category)) return list;      //un indigram n'a pas d'enfant
	        foreach (var t in indiagram.Children)           //une categorie si, on parcours les fils
	        {
	            list.Add(t);                                //on les ajoute
	            if (t is Category)
	            {
	                return AddChildren(list, t);            //et si les fils sont des categories on rappel avec la meme liste
	            }
	        }
	        return list;                                    //si tous les fils sont des indiagrames on ne rappel pas la fonction
	    }
	}
}

