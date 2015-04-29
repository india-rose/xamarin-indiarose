using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IndiaRose.Data.Model;
using PCLStorage;
using System.Xml;
using System.Xml.Linq;
using Storm.Mvvm.Inject;

namespace IndiaRose.Storage
{
    class XmlService : IXmlService
    {
        //TODO path a complteter
        private const string Path = ""; //dossier de destination à rajouter

        private static async void Initialize(IFolder path)
        {
            var files = await path.GetFilesAsync();

            foreach (var t in files)
            {
                FillIndiagram(t.Path);
            }
            //*CreateData(list);
        }

        /*public static void CreateData(List<Indiagram> list)
        {
            foreach (var t in list)
            {
                LazyResolver<ICollectionStorageService>.Service.Create(t);
            }
        }*/
        
        public static Indiagram FillIndiagram(string path)
        {
           
            XDocument xd = new XDocument(path);
            //initialise les premiers noeud en fonction de si le doc concerne un indiagram
            XElement xe = xd.Element("indiagram");
            //... ou une categorie
            if (xd.FirstNode.ToString().StartsWith("<category>"))
            {
                xe = xd.Element("category");
            }

            //instancie le futur return
            Indiagram indiagram = new Indiagram();


            foreach (var t in xe.Nodes())
            {
                var element = (XElement) t;

                //prend les champs text picture et sound
                if (element.ToString().StartsWith("<text>"))
                {
                    indiagram.Text = element.Value;
                }
                if (element.ToString().StartsWith("<picture>"))
                {
                    indiagram.ImagePath = element.Value;
                }
                if (element.ToString().StartsWith("<sound>"))
                {
                    indiagram.SoundPath = element.Value;
                }
                //si le noeud indiagrams est trouve
                if (element.ToString().StartsWith("<indiagrams>"))
                {
                    //on instancie une nouvelle liste d'indiagram
                    var list = new List<Indiagram>();

                    //et pour tout element dans ce noeud on rappel la fonction pour initialiser les fils
                    foreach (var xNode in element.Nodes())
                    {
                        var u = (XElement) xNode;
                        //TODO PATH A FIXER
                        list.Add(FillIndiagram(Path + u.Value));
                    }

                    //on recopie dans une nouvelle instance d'une category on l'on y fait les liens pere/fils
                    var category = new Category(indiagram);
                    foreach (var v in list)
                    {
                        category.Children.Add(v);
                        v.Parent = category;
                    }
                    LazyResolver<ICollectionStorageService>.Service.Create(category);
                    return category;

                }
            }
            LazyResolver<ICollectionStorageService>.Service.Create(indiagram);
            return indiagram;
        }
    }
}
