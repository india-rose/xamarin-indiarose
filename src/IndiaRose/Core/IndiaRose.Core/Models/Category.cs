using System.Collections.Generic;

namespace IndiaRose.Core.Models
{
	public class Category : Indiagram
	{
		public List<Indiagram> Children { get; set; }

		public override bool IsCategory => true;

		public override bool HasChildren => Children.Count > 0;

		public Category()
		{
			Children = new List<Indiagram>();
		}
	}
}