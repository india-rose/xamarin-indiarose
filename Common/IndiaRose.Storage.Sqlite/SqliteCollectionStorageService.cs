using System;
using System.Collections.Generic;
using IndiaRose.Data.Model;
using IndiaRose.Storage.Sqlite.Model;
using SQLite.Net.Async;

namespace IndiaRose.Storage.Sqlite
{
	public class SqliteCollectionStorageService : ICollectionStorageService
	{
		public void Create(Indiagram indiagram)
		{
            throw new NotImplementedException();
		}

		public void Update(Indiagram indiagram)
		{
			throw new NotImplementedException();
		}

		public void Delete(Indiagram indiagram)
		{
			throw new NotImplementedException();
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
