using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Core.Models
{
	public class Indiagram
	{
		public int Id { get; set; }

		public string ImagePath { get; set; }

		public string SoundPath { get; set; }

		public string Text { get; set;} 

		public Category Parent { get; set; }

		public bool IsEnabled { get; set; }

		public int Position { get; set; }

		public bool HasCustomSound => SoundPath != null;

		public virtual bool IsCategory => false;

		public virtual bool HasChildren => false;
	}
}
