using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace IndiaRose.Data.Model
{
    [Table("Category")]
    public class CategorySql : IndiagramSql
    {
        [Column("_children")]
        private IList<IndiagramSql> Children { get; set; }
    }
}
