using System.Collections.ObjectModel;

namespace IndiaRose.Data.Model
{
    /// <summary>
    /// Classe représentant une Catégorie
    /// Indiagram contenant d'autres Indiagrams
    /// </summary>
	public class Category : Indiagram
	{
		private readonly ObservableCollection<Indiagram> _children = new ObservableCollection<Indiagram>();

        /// <summary>
        /// Collection des Indiagrams contenus
        /// </summary>
		public ObservableCollection<Indiagram> Children
		{
			get { return _children; }
		}

        /// <summary>
        /// Toujours vrai
        /// </summary>
		public override bool IsCategory
		{
			get { return true; }
		}

        /// <summary>
        /// Vrai si la catégorie contient d'autres Indiagrams
        /// </summary>
	    public override bool HasChildren
	    {
		    get { return Children.Count > 0; }
	    }

        /// <summary>
        /// Constructeur vide
        /// </summary>
		public Category()
		{
		}

        /// <summary>
        /// Construit une catégorie et y attribue des Indiagrams enfants
        /// </summary>
        /// <param name="children">Les Indiagrams à contenir</param>
		public Category(ObservableCollection<Indiagram> children)
		{
			_children = children;
		}
	}
}
