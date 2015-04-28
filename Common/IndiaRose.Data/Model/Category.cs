using System.Collections.Generic;

namespace IndiaRose.Data.Model
{
	public class Category : Indiagram
	{
		private readonly List<Indiagram> _children = new List<Indiagram>();

		public List<Indiagram> Children
		{
			get { return _children; }
		}
		public override bool IsCategory
		{
			get { return true; }
		}

	    public override bool HasChildren()
	    {
	        return Children.Count > 0;
	    }
	    public Category()
		{	
		}
		public Category(string text, string imagePath, string soundPath = null) : base(text, imagePath, soundPath)
		{

		}
	    public Category(string text, string imagePath, Category a): base(text,imagePath,a)
        {
        }

	    public Category(Indiagram cloneIndiagram) : base(cloneIndiagram)
	    {

	    }
	}
}
