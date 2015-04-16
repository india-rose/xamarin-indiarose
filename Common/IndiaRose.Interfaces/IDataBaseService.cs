using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using SQLite;

namespace IndiaRose.Interfaces
{
    public interface IDataBaseService
    {
        String DbPath { get; set; }

        void Add(IndiagramSql a);

        IndiagramSql Edit(IndiagramSql a);

        IndiagramSql Delete(IndiagramSql a);

        void ResetDataBase();

        IndiagramSql SearchById(int id);


    }
}
