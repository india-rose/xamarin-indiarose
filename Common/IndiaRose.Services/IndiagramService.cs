/*

using System;
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

        public List<IndiagramSql> GetList(IndiagramSql a)
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<IndiagramSql>();
            var liste = new List<IndiagramSql>();
            foreach (var t in table)
            {
                if (t.Parent.Equals(a.Parent))
                {
                    liste.Add(t);
                }
            }
            return liste;
        }

        public IndiagramSql GetIndiagramSql(int id)
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<IndiagramSql>();

            return table.FirstOrDefault(t => t.Id == id);
        }

        public void Edit(IndiagramSql a)
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
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
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<CategorySql>();

            return Enumerable.FirstOrDefault((from t in table where t.Id == a.Id select t.Children));
        }
    }
}
 * 
 * */
