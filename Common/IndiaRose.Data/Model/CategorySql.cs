/*using System;
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
        public List<IndiagramSql> Children { get; set; }


        public CategorySql(int version,
            string text, 
            string imagepath, 
            string soundpath, 
            int parent)
            : base(version,text,imagepath,soundpath,parent)
        {
            //Test
            Children = new List<IndiagramSql>();
        }

        public CategorySql()
        {
            //throw new NotImplementedException();
        }
    }
}
*/