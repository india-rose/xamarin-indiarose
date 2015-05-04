using System.ComponentModel;
using IndiaRose.Data.Model;
using SQLite.Net.Attributes;

namespace IndiaRose.Storage.Sqlite.Model
{
	[Table("Indiagram")]
	class IndiagramSql
	{
		public const int ROOT_PARENT = -0x2a;

		[PrimaryKey, AutoIncrement, Column("id")]
		public int Id { get; set; }

		[Column("version")]
		public int Version { get; set; }

		[Column("text")]
		public string Text { get; set; }

		[Column("imagePath")]
		public string ImagePath { get; set; }

		[Column("soundPath")]
		public string SoundPath { get; set; }

		[Column("parentId")]
		public int ParentId { get; set; }

		[Column("position")]
		public int Position { get; set; }

		[Column("isEnabled")]
		public int IsEnabled { get; set; }

		[Column("isCategory")]
		public int IsCategory { get; set; }

		public Indiagram ToModel()
		{
			Indiagram result = IsCategory != 0 ? new Category() : new Indiagram();

			result.Id = Id;
			result.Text = Text;
			result.ImagePath = ImagePath;
			result.SoundPath = SoundPath;
			result.Position = Position;
			result.IsEnabled = IsEnabled != 0;
			result.Parent = null;

			return result;
		}

		public void FromModel(Indiagram indiagram)
		{
			Text = indiagram.Text;
			ImagePath = indiagram.ImagePath;
			SoundPath = indiagram.SoundPath;
			Position = indiagram.Position;
			IsEnabled = indiagram.IsEnabled ? 1 : 0;
			IsCategory = indiagram.IsCategory ? 1 : 0;
			ParentId = indiagram.Parent == null ? ROOT_PARENT : indiagram.Parent.Id;
		}
	}
}