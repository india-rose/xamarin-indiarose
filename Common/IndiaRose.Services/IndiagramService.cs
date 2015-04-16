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

        }

        public IndiagramSql GetIndiagramSql(int id)
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<IndiagramSql>();

            foreach (var t in table)
            {
                if (t.Id == id)
                {
                    return t;
                }
            }

            new NotImplementedException();
        }

        public void Edit(IndiagramSql a)
        {
            
        }

        public List<IndiagramSql> SearchCategorie(CategorySql a)
        {
            var db = new SQLiteConnection(DataBaseService.DbPath);
            var table = db.Table<CategorySql>();

            foreach (var t in table)
            {
                if (t.Id == a.Id)
                {
                    return t;
                }
            }

            new NotImplementedException();
        }
    }
}
