using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace IndiaRose.Data.Model
{
    [Table("Indiagram")]
    public class IndiagramSql
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
    }

}
