using SQLite.Net.Attributes;

namespace IndiaRose.Storage.Sqlite.Model
{
    [Table("Category")]
    class CategorySql : IndiagramSql
    {
        public CategorySql(int version,
            string text, 
            string imagepath, 
            string soundpath, 
            int parent)
            : base(version,text,imagepath,soundpath,parent)
        {
        }

        public CategorySql()
        {
            //throw new NotImplementedException();
        }
    }
}