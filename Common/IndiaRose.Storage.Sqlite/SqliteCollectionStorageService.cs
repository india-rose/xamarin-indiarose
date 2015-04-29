using System.Collections.Generic;
using System.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Storage.Sqlite.Model;
using SQLite.Net;
using SQLite.Net.Interop;
using Storm.Mvvm.Inject;

namespace IndiaRose.Storage.Sqlite
{
	public class SqliteCollectionStorageService : ICollectionStorageService
	{
		//Attributes 
		private readonly ISQLitePlatform _platform;
		private readonly string _dbPath;
		private SQLiteConnection _connection;

		//Database

		protected SQLiteConnection Connection
		{
			get { return _connection ?? (_connection = OpenDatabase()); }
		}

		public SqliteCollectionStorageService(ISQLitePlatform platform)
		{
			_platform = platform;
			_dbPath = LazyResolver<IStorageService>.Service.DatabasePath;
		}

		private SQLiteConnection OpenDatabase()
		{
			var database = new SQLiteConnection(_platform, _dbPath);
			database.CreateTable<IndiagramSql>();
			database.CreateTable<CategorySql>();
			return database;
		}

		//Methods convert SQL <-> C#

		private IndiagramSql GetIndiagramSql(Indiagram indiagram)
		{
			//Cherche dans la base de donnees l'indiagramme pour entre autre recuperer les liens pere/fils si besoin
			if (indiagram is Category)
			{
				return Connection.Table<CategorySql>().SingleOrDefault(t => t.Id == indiagram.Id);
			}

			return Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Id == indiagram.Id);
		}

		private Indiagram GetIndiagramFromSql(IndiagramSql indiagram)
		{
			//Instancie l'indiagramme sans les fils et peres
			//Des methodes sont prevues si on en a besoin
			if (indiagram is CategorySql)
			{
				var c = new Category
				{
                    Id = indiagram.Id,
                    Text = indiagram.Text,
                    ImagePath = indiagram.ImagePath,
                    SoundPath = indiagram.SoundPath,
                    Position = indiagram.Position,
                    IsEnabled = indiagram.Enabled != 0
				};

                if (indiagram.Parent != 0)
			    {
                    c.Parent = (Category)GetIndiagramFromSql(SearchById(true, indiagram.Parent));
                    c.Parent.Children.Add(c);
			    }
			    else
			    {
			        c.Parent = null;
			    }

                return c;

			}
			var i = new Indiagram()
			{
				Id = indiagram.Id,
				Text = indiagram.Text,
				ImagePath = indiagram.ImagePath,
				SoundPath = indiagram.SoundPath,
				Position = indiagram.Position,
                IsEnabled = indiagram.Enabled != 0,
			};
            if (indiagram.Parent != 0)
            {
                i.Parent = (Category) GetIndiagramFromSql(SearchById(false, indiagram.Parent));
                i.Parent.Children.Add(i);
            }
            else
            {
                i.Parent = null;
            }

            return i;

		}

        //TODO delete after test
        public void DropTable()
        {
            Connection.DropTable<IndiagramSql>();
            Connection.DropTable<CategorySql>();
            Connection.CreateTable<IndiagramSql>();
            Connection.CreateTable<CategorySql>();
        }

		//Interface

		public void Create(Indiagram indiagram)
		{
			//Instancie un IndiagrammeSQL ou CategorySQL pour l'ajouter a la base de donnees

			if (indiagram is Category)
			{
                var temp = new CategorySql
				{
					ImagePath = indiagram.ImagePath,
					SoundPath = indiagram.SoundPath,
					Text = indiagram.Text,
                    Enabled = indiagram.IsEnabled ? 1 : 0
				};

                if ((indiagram.Parent != null) && (GetIndiagramSql(indiagram.Parent) != null))
				{
					temp.Parent = (GetIndiagramSql(indiagram.Parent)).Id;
                    indiagram.Parent.Children.Add(indiagram);
                    Update(indiagram.Parent);
				}
				else
				{
					temp.Parent = 0;
				}
			    Connection.Insert(temp);
				indiagram.Id = indiagram.Position = temp.Position = temp.Id;
                Update(indiagram);
                
                
			}
			else
			{
				var temp = new IndiagramSql
				{
					ImagePath = indiagram.ImagePath,
					SoundPath = indiagram.SoundPath,
					Text = indiagram.Text,
                    Enabled = indiagram.IsEnabled ? 1 : 0
				};

				if ((indiagram.Parent != null)&&(GetIndiagramSql(indiagram.Parent)!=null))
				{
					temp.Parent = (GetIndiagramSql(indiagram.Parent)).Id;
                    indiagram.Parent.Children.Add(indiagram);
                    Update(indiagram.Parent);
				}
				else
				{
					temp.Parent = 0;
				}

				temp.Position = temp.Id;
				Connection.Insert(temp);
                indiagram.Id = indiagram.Position = temp.Position = temp.Id;
                Update(indiagram);
			}
		}

		public void Delete(Indiagram indiagram)
		{
			//supprime dans la table adequate
		    if (indiagram.Parent != null)
		    {
		        var parentNull = indiagram.Parent;
		        if (parentNull != null) parentNull.Children.Remove(indiagram);
		        Update(indiagram.Parent);
		    }

		    if (indiagram is Category)
		    {
		        var category = indiagram as Category;
				Connection.Delete<CategorySql>(category.Id);
			    foreach (var t in category.Children.ToList())
			    {
                    Delete(t);
			        category.Children.Remove(t);
			    }
			}
			else
			{
				Connection.Delete<IndiagramSql>(indiagram.Id);
			}
		}

		public void Update(Indiagram indiagram)
		{
			if (indiagram is Category)
			{
				var query = Connection.Table<CategorySql>().SingleOrDefault(t => t.Id == indiagram.Id);

				if (query == null) return;
				query.Text = indiagram.Text;
				query.ImagePath = indiagram.ImagePath;
				query.SoundPath = indiagram.SoundPath;
				query.Position = indiagram.Position;
                query.Enabled = indiagram.IsEnabled ? 1 : 0;

			    if (indiagram.Parent != null)
			    {
			        query.Parent = GetIndiagramSql(indiagram.Parent).Id;
			    }
			    else
			    {
                    query.Parent = 0;
			    }

                /*query.Children = new List<IndiagramSql>();
			        foreach (var children in indiagram.Children)
			        {
			            query.Children.Add(GetIndiagramSql(children));
			        }*/

			    Connection.Update(query);
			}
			else
			{
				var query = Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Id == indiagram.Id);

				if (query == null) return;
				query.Text = indiagram.Text;
				query.ImagePath = indiagram.ImagePath;
				query.SoundPath = indiagram.SoundPath;
				query.Position = indiagram.Position;
                query.Enabled = indiagram.IsEnabled ? 1 : 0;

                if (indiagram.Parent != null)
                {
                    query.Parent = GetIndiagramSql(indiagram.Parent).Id;
                }
                else
                {
                    query.Parent = 0;
                }

				Connection.Update(query);
			}
		}

		public List<Indiagram> GetTopLevel()
		{
			Indiagram indiagram;
			var list = new List<Indiagram>();
			var table = Connection.Table<CategorySql>();
			var table2 = Connection.Table<IndiagramSql>();

			foreach (var v in table.Where(i => i.Parent == 0))
			{
				indiagram = GetIndiagramFromSql(v);

				list.Add(indiagram);

			}

			foreach (var i in table2.Where(i => i.Parent == 0))
			{
				indiagram = GetIndiagramFromSql(i);

				list.Add(indiagram);
			}

			return list;
		}

	    public List<Indiagram> GetFullCollection()
	    {
	        var list = new List<Indiagram>();
	        var list2 = Connection.Table<IndiagramSql>();
	        var list3 = Connection.Table<CategorySql>();

	        foreach (var table in list2)
	        {
	            list.Add(GetIndiagramFromSql(table));
	        }
	        foreach (var table in list3)
	        {
	            list.Add(GetIndiagramFromSql(table));
	        }

	        return list;
	    }

        private IndiagramSql SearchById(bool isCategory, int id)
        {
            //Cherche dans la base de donnees l'indiagramme pour entre autre recuperer les liens pere/fils si besoin
            if (isCategory)
            {
                return Connection.Table<CategorySql>().SingleOrDefault(t => t.Id == id);
            }

            return Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Id == id);
        }
		/*public List<Indiagram> GetFullCollection()
		{
			var list = new List<Indiagram>();
		    List<Indiagram> current;
			//Parcourir le topLevel dans un premier temps
			foreach (var t in GetTopLevel())
			{
			    current = new List<Indiagram>();
			    list.AddRange(GetFullCollectionRec(current, t));/*ajouter les enfants avec la methode recursive
                                             *(1er appel)
                                             * 
                                             * 
                                             *
			}
			return list;

		}
        //TODO Bug for children
		private static List<Indiagram> GetFullCollectionRec(List<Indiagram> list, Indiagram indiagram)
		{
            list.Add(indiagram); 

			if (!(indiagram is Category)){
                return list;      //un indigram n'a pas d'enfant
                }
			foreach (var t in indiagram.Children)           //une categorie si, on parcours les fils
			{
				list.Add(t);                                //on les ajoute
				if (t is Category)
				{
					return GetFullCollectionRec(list, t);   //et si les fils sont des categories on rappel avec la meme liste
				}
			}
			return list;                                    //si tous les fils sont des indiagrames on ne rappel pas la fonction
		}*/
		public List<Indiagram> GetChildren(Indiagram parent)
		{
			if (!(parent is Category)) return new List<Indiagram>();

			var current = new List<Indiagram>();
		    var i = GetIndiagramSql(parent).Id;
		    var list = Connection.Table<CategorySql>();

		    foreach (var table in list.Where(t => t.Parent == i))
		    {
		        current.Add(GetIndiagramFromSql(table));
		    }

            var list2 = Connection.Table<IndiagramSql>();

            foreach (var table in list2.Where(t => t.Parent == i))
            {
                current.Add(GetIndiagramFromSql(table));
            }

		    return current;
		}

		//others methods


	}

	//TODO void Update(Indiagram indigram) & Test Service
	/*"useless" methods ?
	 * 
	 *
	 * 
	 *
	 *
	 * 
	 *  pas forcement utile
	 * 
	 * private IndiagramSql SearchByIdSql(int id)
	 *    {
	 *       return Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Id == id);
	 *    }
	 * 
			public void ChangeCategory(Indiagram indiagram, Category category)
		{
			indiagram.Parent = category;
			Update(indiagram);
		}
	 * *


	 *
	 * 
	 * A voir si besoin
	 * 
	 *
	 *private List<Indiagram> AddCategory(CategorySql csql)
{
	Indiagram i;
	List<Indiagram> list = new List<Indiagram>();

	foreach (var c in csql.Children)
	{
		list.Add(GetIndiagramFromSql(c));
	}

	return list;
}

    
	 * peut etre inutile
	 * 
	 *         private List<Indiagram> SearchChildren(List<Indiagram> topLevel, Indiagram parent)
		{
			foreach (var table in topLevel)
			{
				if (table is Category)
				{
					if (table.Equals(parent))
					{
						return table.Children;
					}
					return SearchChildren(table.Children, parent);
				} 
			}

			return null;
		}

		private Indiagram SearchCategory(CategorySql p0)
		{
			throw new NotImplementedException();
		}*/
}

