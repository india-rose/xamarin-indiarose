
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
        List<IndiagramSql> GetList(IndiagramSql a);

        IndiagramSql GetIndiagramById(int id);

        void Edit(IndiagramSql a);

        List<IndiagramSql> SearchCategorie(CategorySql a);

        List<CategorySql> GetAllCategorySql();

        List<IndiagramSql> GetChildren(CategorySql parent);

        CategorySql GetParent(IndiagramSql children);
    }
}
