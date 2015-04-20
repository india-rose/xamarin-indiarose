using System;
using System.Collections.Generic;
using System.IO;
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

		public List<Indiagram> GetTopLevel()
		{
			throw new NotImplementedException();
		}

		public List<Indiagram> GetChildren(Indiagram parent)
		{
			throw new NotImplementedException();
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
