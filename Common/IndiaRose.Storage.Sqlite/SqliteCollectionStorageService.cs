using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Storage.Sqlite.Model;

using SQLite;
using SQLite.Net;
using SQLite.Net.Async;

namespace IndiaRose.Storage.Sqlite
{
	public class SqliteCollectionStorageService : ICollectionStorageService
	{
        //public string DbPath { get; set; } ??

	    public void Create(Indiagram indiagram)
	    {

            throw new NotImplementedException();

            //TODO connexion impossible
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
	            //TODO add avec connexion
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

	    public IndiagramSql GetIndiagramSql(Indiagram indiagram)
	    {
            throw new NotImplementedException();

           /*var temp = from s in db.Table<Indiagram>()
                        where
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
            List<Indiagram> list = new List<Indiagram>(), list2 = new List<Indiagram>();

            /*var db = new SQLiteConnection(dbPath);
            var table = db.Table<CategorySql>();

            foreach (var v in table)
            {
                list2.Add(SearchCategory(v));
            }

            foreach (var i in list2)
            {
                foreach (var j in list2)
                {
                    foreach (var k in j.Children)
                    {
                        if (i.Equals(k))
                        {
                            i.Parent = j;
                        }
                    }
                }
            }

            foreach (var i in list2)
            {
                if (i.Parent != null)
                {
                    list.Add(i);
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

	    public List<Indiagram> AddChildren(List<Indiagram> list, Indiagram indiagram)
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
