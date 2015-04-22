using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Storage.Sqlite.Model;

using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace IndiaRose.Storage.Sqlite
{
    public class SqliteCollectionStorageService : ICollectionStorageService
    {
        //Attributes 

        private const string DB_PATH = "india.sqlite";
        private readonly ISQLitePlatform _platform;
        private SQLiteConnection _connection;

        //Database

        protected SQLiteConnection Connection
        {
            get { return _connection ?? (_connection = OpenDatabase()); }
        }

        public SqliteCollectionStorageService(ISQLitePlatform platform)
        {
            _platform = platform;
        }

        private SQLiteConnection OpenDatabase()
        {
            var database = new SQLiteConnection(_platform, DB_PATH);
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
                return Connection.Table<CategorySql>().SingleOrDefault(t => t.Text == indiagram.Text &&
                                                                            t.ImagePath == indiagram.ImagePath &&
                                                                            t.SoundPath == indiagram.SoundPath &&
                                                                            t.Position == indiagram.Position);
            }

            return Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Text == indiagram.Text &&
                                                                         t.ImagePath == indiagram.ImagePath &&
                                                                         t.SoundPath == indiagram.SoundPath &&
                                                                         t.Position == indiagram.Position);
        }

        private static Indiagram GetIndiagramFromSql(IndiagramSql indiagram)
        {
            //Instancie l'indiagramme sans les fils et peres
            //Des methodes sont prevues si on en a besoin
            if (indiagram is CategorySql)
            {
                var csql = (CategorySql)indiagram;
                var c = new Category()
                {
                    Text = csql.Text,
                    ImagePath = csql.ImagePath,
                    SoundPath = csql.SoundPath,
                    Position = csql.Position,
                };

            }
            var i = new Indiagram
            {
                Text = indiagram.Text,
                ImagePath = indiagram.ImagePath,
                SoundPath = indiagram.SoundPath,
                Position = indiagram.Position,
            };

            return i;
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
                    Text = indiagram.Text
                };
                temp.Position = temp.Id;
                temp.Parent = (GetIndiagramSql(indiagram.Parent)).Id;
                Connection.Insert(temp);
            }
            else
            {
                var temp = new IndiagramSql
                {
                    ImagePath = indiagram.ImagePath,
                    SoundPath = indiagram.SoundPath,
                    Text = indiagram.Text
                };
                temp.Position = temp.Id;
                Connection.Insert(temp);
            }
        }

        public void Delete(Indiagram indiagram)
        {
            //supprime dans la table adequate
            var current = GetIndiagramSql(indiagram);
            if (current is CategorySql)
            {
                Connection.Delete<CategorySql>(current.Id);
            }
            else
            {
                Connection.Delete<IndiagramSql>(current.Id);
            }
        }

        public void Update(Indiagram indiagram)
        {
            throw new NotImplementedException();

            if (indiagram is Category)
            {
                var query = Connection.Table<CategorySql>().SingleOrDefault(t => t.Text == indiagram.Text ||
                    t.ImagePath == indiagram.ImagePath || t.SoundPath == indiagram.SoundPath ||
                    t.Position == indiagram.Position);
                query.Text = indiagram.Text;
                query.ImagePath = indiagram.ImagePath;
                query.SoundPath = indiagram.SoundPath;
                //query.Children.CopyTo(indiagram.Children);
                query.Position = indiagram.Position;
                Connection.Update(query);
            }
            else
            {
                var query = Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Text == indiagram.Text ||
                    t.ImagePath == indiagram.ImagePath || t.SoundPath == indiagram.SoundPath ||
                    t.Position == indiagram.Position);
                query.Text = indiagram.Text;
                query.ImagePath = indiagram.ImagePath;
                query.SoundPath = indiagram.SoundPath;
                query.Position = indiagram.Position;
                Connection.Update(query);
            }
        }

        public List<Indiagram> GetTopLevel()
        {
            Indiagram indiagram;
            var list = new List<Indiagram>();
            var table = Connection.Table<CategorySql>();
            var table2 = Connection.Table<IndiagramSql>();

            foreach (var v in table)
            {
                indiagram = GetIndiagramFromSql(v);

                if (indiagram.Parent != null)
                {
                    list.Add(indiagram);
                }
            }

            foreach (var i in table2.Where(i => i.Parent != 0))
            {
                indiagram = GetIndiagramFromSql(i);

                list.Add(indiagram);
            }

            return list;
        }

        public List<Indiagram> GetFullCollection()
        {
            var list = new List<Indiagram>();
            //Parcourir le topLevel dans un premier temps
            foreach (var t in GetTopLevel())
            {
                list = GetFullCollectionRec(list, t);/*ajouter les enfants avec la methode recursive
                                             *(1er appel)
                                             * 
                                             * 
                                             */
            }
            return list;

        }

        private static List<Indiagram> GetFullCollectionRec(List<Indiagram> list, Indiagram indiagram)
        {
            if (!(indiagram is Category)) return list;      //un indigram n'a pas d'enfant
            foreach (var t in indiagram.Children)           //une categorie si, on parcours les fils
            {
                list.Add(t);                                //on les ajoute
                if (t is Category)
                {
                    return GetFullCollectionRec(list, t);   //et si les fils sont des categories on rappel avec la meme liste
                }
            }
            return list;                                    //si tous les fils sont des indiagrames on ne rappel pas la fonction
        }

        public List<Indiagram> GetChildren(Indiagram parent)
        {
            //throw new NotImplementedException();

            //TODO connexion impossible
            if (!(parent is Category)) return new List<Indiagram>();
            var current = new List<Indiagram>();
            CategorySql parentSql = (CategorySql)GetIndiagramSql(parent);
            List<IndiagramSql> listChildren = parentSql.Children;
            foreach (var t in listChildren)
            {
                current.Add(GetIndiagramFromSql(t));
            }
            return current;

            /*List<Indiagram> list = GetTopLevel();

            return SearchChildren(list, parent);*/
        }

    }


    /*
     *TODO void Update(Indiagram indigram) & Test Service
     * 
     * "useless" methods ?
     * 
     * utile mais pas encore utilisee
     * 
     * private IndiagramSql SearchByIdSql(int id)
            {
                return Connection.Table<IndiagramSql>().SingleOrDefault(t => t.Id == id);
            }
     * */

    /*
     * 
     *  pas forcement utile
     * 
     * 
            public void ChangeCategory(Indiagram indiagram, Category category)
        {
            indiagram.Parent = category;
            Update(indiagram);
        }
     * */


    /*
     * 
     * A voir si besoin
     * 
     */
    /*private List<Indiagram> AddCategory(CategorySql csql)
{
    Indiagram i;
    List<Indiagram> list = new List<Indiagram>();

    foreach (var c in csql.Children)
    {
        list.Add(GetIndiagramFromSql(c));
    }

    return list;
}*/

    /*
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
	    }*/

    /*        //TODO 
        private Indiagram SearchCategory(CategorySql p0)
        {
            throw new NotImplementedException();
        }*/
}

