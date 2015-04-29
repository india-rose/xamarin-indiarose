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
			//TODO : a remplire
		}
		public Category(string text, string imagePath, string soundPath = null) : base(text, imagePath, soundPath)
		{
			//TODO : a remplire
		}
	    public Category(string text, string imagePath, Category a): base(text,imagePath,a)
        {
			//TODO : a remplire
        }

	    public Category(Indiagram cloneIndiagram) : base(cloneIndiagram)
	    {
			//TODO : a remplire
	    }
	}
}
