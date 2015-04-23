using System.Collections.Generic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace IndiaRose.Storage.Sqlite.Model
{
    [Table("Category")]
    public class CategorySql : IndiagramSql
    {
        [OneToMany]
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