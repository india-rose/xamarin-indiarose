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
        private int Id { get; set; }
        [Column("_version")]
        private int Version { get; set; }
        [Column("_text")]
        private String Text { get; set; }
        [Column("_imagePath")]
        private String ImagePath { get; set; }
        [Column("_soundPath")]
        private String SoundPath { get; set; }
        [Column("_parent")]
        private int Parent { get; set; }
    }

}
