/*
 * 
 * 
 * using System;
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
        public string DbPath { get; set; }


        public DataBaseService(string dbPath)
        {
            DbPath = dbPath;
            /*TODO initialisation database with database string path and img
            var db = new SQLiteConnection(DbPath);
            //create or repalce
            db.CreateTable<IndiagramSql>();
            db.CreateTable<CategorySql>();
             * 
             */
/* COMMENT
        }


        public void Add(IndiagramSql a)
        {
            var db = new SQLiteConnection(DbPath);

            if (a is CategorySql)
            {
               db.Table<CategorySql>();
            }
            else
            {
                db.Table<IndiagramSql>();
            }
            db.Insert(a);
            db.Close();

        }

        public void Replace(IndiagramSql current,IndiagramSql _new)
        {
            if (current.GetType() == _new.GetType()) return;
            Delete(current);
            Add(_new);
        }

        public async void Delete(IndiagramSql a)
        {
            var db = new SQLiteAsyncConnection(DbPath);
            await db.DeleteAsync(a);

        }

        public async void Update(IndiagramSql a)
        {
            var db = new SQLiteAsyncConnection(DbPath);
            await db.UpdateAsync(a);
        }


        public void ResetDataBase()
        {
            //TODO droptabase and reset path, img...
        }


        public async Task<IndiagramSql> SearchById(int id)
        {
            var db = new SQLiteAsyncConnection(DbPath);

            var query = db.Table<IndiagramSql>().Where(x => x.Id == id);
            var result = await query.ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<IndiagramSql>> GetAll()
        {
            var db = new SQLiteAsyncConnection(DbPath);

            var query = db.Table<IndiagramSql>();
            var result = await query.ToListAsync();

            return result;
        }
    }
}

*/