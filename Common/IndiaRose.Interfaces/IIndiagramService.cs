using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    public interface IIndiagramService
    {
        //liste de sa catégorie
        //List<IndiagramSql> GetList();

        //retourne indiagramsql
        IndiagramSql GetIndiagramSql(int id);

        //modifie indiagramsql
        void Edit(IndiagramSql a);

        //retourne list<indiagramsql> de la catégorie donnée
        List<IndiagramSql> SearchCategorie(CategorySql a);
    }
}
