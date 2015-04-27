using System;
using IndiaRose.Data.Model;
using SQLite.Net.Attributes;

namespace IndiaRose.Storage.Sqlite.Model
{
    [Table("Indiagram")]
    class IndiagramSql
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [Column("_version")]
        public int Version { get; set; }
        [Column("_text")]
        public String Text { get; set; }
        [Column("_imagePath")]
        public String ImagePath { get; set; }
        [Column("_soundPath")]
        public String SoundPath { get; set; }
        [Column("_parent")]
        public int Parent { get; set; }
        [Column ("_position")]
        public int Position { get; set;}
        [Column("_isEnabled")]
        public int Enabled { get; set; }

      public IndiagramSql(int version,string text, string imagepath, string soundpath, int parent)
        {
            Version = version;
            Text = text;
            ImagePath = imagepath;
            SoundPath = soundpath;
            Parent = parent;
        }

        public IndiagramSql(Indiagram a)
        {
            Text = a.Text;
            ImagePath = a.ImagePath;
            SoundPath = a.SoundPath;
            if (a.Parent != null)
            {
                Parent = new IndiagramSql(a.Parent).Id;
            }
            else
            {

            }
        }

        public IndiagramSql()
        {

        }
    }
    

}
