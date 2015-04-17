using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<IndiagramSql> GetList()
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<IndiagramSql>();

            foreach (var t in table)
            {

            }
        }

        public IndiagramSql GetIndiagramSql(int id)
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<IndiagramSql>();

            return table.FirstOrDefault(t => t.Id == id);
        }

        public void Edit(IndiagramSql a)
        {

        }

        public List<IndiagramSql> SearchCategorie(CategorySql a)
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<CategorySql>();

            return Enumerable.FirstOrDefault((from t in table where t.Id == a.Id select t.Children));
        }
    }
}
