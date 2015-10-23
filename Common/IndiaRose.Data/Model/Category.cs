using System.Collections.ObjectModel;

namespace IndiaRose.Data.Model
{
	public class Category : Indiagram
	{
		private readonly ObservableCollection<Indiagram> _children = new ObservableCollection<Indiagram>();

		public ObservableCollection<Indiagram> Children
		{
			get { return _children; }
		}

		public override bool IsCategory
		{
			get { return true; }
		}

	    public override bool HasChildren
	    {
		    get { return Children.Count > 0; }
	    }

		public Category()
		{
		}

		public Category(ObservableCollection<Indiagram> children)
		{
			_children = children;
		}
	}
}
