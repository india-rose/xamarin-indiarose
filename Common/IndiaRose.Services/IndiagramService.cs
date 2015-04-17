/*
 * 
 * 
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using SQLite;

namespace IndiaRose.Services
{
    class IndiagramService : IIndiagramService
    {
        private DataBaseService data;

        public List<IndiagramSql> GetList(IndiagramSql a)
        {
            var db = new SQLiteConnection(data.DbPath);
            var table = db.Table<IndiagramSql>();
            return table.Where(t => t.Parent.Equals(a.Parent)).ToList();
        }

        public IndiagramSql GetIndiagramById(int id)
        {
            return data.SearchById(id).Result;
        }

        public void Edit(IndiagramSql a)
        {
            var db = new SQLiteConnection(data.DbPath);
            var query = db.Table<IndiagramSql>().SingleOrDefault(t => t.Id == a.Id);
            query.ImagePath = a.ImagePath;
            query.SoundPath = a.SoundPath;
            query.Text = a.Text;
            query.Parent = a.Parent;
            query.Version = a.Version;

            db.Update(query);
            db.Close();
        }

        public List<IndiagramSql> SearchCategorie(CategorySql a)
        {
            var db = new SQLiteConnection(data.DbPath);
            var table = db.Table<CategorySql>();

            return Enumerable.FirstOrDefault((from t in table where t.Id == a.Id select t.Children));
        }

        public List<CategorySql> GetAllCategorySql()
        {
            var db = new SQLiteConnection(data.DbPath);
            var table = db.Table<CategorySql>();
            return table.ToList();
        }

        public List<IndiagramSql> GetChildren(CategorySql parent)
        {
            return parent.Children;
        }

        public CategorySql GetParent(IndiagramSql children)
        {
            return data.SearchById(children.Parent).Result as CategorySql;
        }

 
    }
}

*/