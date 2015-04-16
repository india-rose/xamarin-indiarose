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
        List<IndiagramSql> GetList();
        void Edit(IndiagramSql a);
        List<IndiagramSql> SearchCategrorie(CategorySql a);
    }
}
