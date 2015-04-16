using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using SQLite;

namespace IndiaRose.Services
{
    class DataBaseService : IDataBaseService
    {
        public string DbPath;


        public DataBaseService(string dbPath)
        {
            DbPath = dbPath;
            var db = new SQLiteConnection(DbPath);
            //create or repalce
            db.CreateTable<IndiagramSql>();
            db.CreateTable<CategorySql>();
        }


        public void Add(IndiagramSql a)
        {
            var db = new SQLiteConnection(DbPath);
            if (a is CategorySql)
            {
                var table = db.Table<CategorySql>();
            }
            else
            {
                var table = db.Table<IndiagramSql>();
            }
            db.Insert(a);
            db.Close();

        }

        public void Switch(IndiagramSql current,IndiagramSql _new)
        {
        }

        public IndiagramSql Delete(IndiagramSql a)
        {
            throw new NotImplementedException();
        }

        public void ResetDataBase()
        {
            throw new NotImplementedException();
        }

        public IndiagramSql SearchById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
