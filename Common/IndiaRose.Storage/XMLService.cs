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
        private const string Path = ""; //dossier de destination à rajouter

        private async Task<List<IFile>> SearchXml()
        {
            IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(Path);
            List<IFile> xml = new List<IFile>();

            Folders(xml, folder);

            return xml;
        }

        private static async void Folders(List<IFile> list, IFolder path)
        {
            IList<IFolder> folders = await path.GetFoldersAsync();
            foreach (var t in folders)
            {
                Folders(list, t);
            }

            IList<IFile> files = await path.GetFilesAsync();
            list.AddRange(files);
        }

        //TODO test impossible

        public static List<Indiagram> Indiagrams(List<IFile> list)
        {
            List<Indiagram> indiagrams = new List<Indiagram>();
            foreach (var t in list)
            {
                string str = t.ReadAllTextAsync().ToString();
                XmlReader reader = XmlReader.Create(new StringReader(str));
                if (str.StartsWith("<cat"))
                {
                    //categorie
                    var category = new Category();

                    indiagrams.Add(category);
                }
                else
                {
                    //indiagram
                    var indiagram = new Indiagram();


                    indiagrams.Add(indiagram);
                }
            }
            return indiagrams;
        }

        private static Indiagram Create(string path)
        {
            XDocument doc = XDocument.Load(path);

            if (doc.FirstNode.ToString().StartsWith("<c"))
            {
                Category c = new Category
                {
                    Text = doc.Descendants("text").First().Value,
                    ImagePath = doc.Descendants("picture").First().Value,
                    SoundPath = doc.Descendants("sound").First().Value
                };

                LazyResolver<ICollectionStorageService>.Service.Create(c);
                return c;
            }

            Indiagram i = new Indiagram
            {
                Text = doc.Descendants("text").First().Value,
                ImagePath = doc.Descendants("picture").First().Value,
                SoundPath = doc.Descendants("sound").First().Value
            };

            LazyResolver<ICollectionStorageService>.Service.Create(i);
            return i;
        }

        public void XmlToSql()
        {
            Task<List<IFile>> files = SearchXml();

        }

        public static Indiagram FillTextAndPaths(string path)
        {
            var xd = XDocument.Load("path");
            var xe = xd.Element("indiagram");
            var india = new Indiagram();
            if (xe == null) return india;
            foreach (var t in xe.Nodes().Cast<XElement>())
            {
                if (t.ToString().StartsWith("<text>"))
                {
                    india.Text = t.Value;
                }
                if (t.ToString().StartsWith("<picture>"))
                {
                    india.ImagePath = t.Value;
                }
                if (t.ToString().StartsWith("<sound>"))
                {
                    india.SoundPath = t.Value;
                }
            }
            return india;
        }

        public static Indiagram LireXml(XDocument xd)
        {
           
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
                        list.Add(LireXml(XDocument.Load(Path+"/base/xml/fr" + u.Value)));
                    }

                    //on recopie dans une nouvelle instance d'une category on l'on y fait les liens pere/fils
                    var category = new Category(indiagram);
                    foreach (var v in list)
                    {
                        category.Children.Add(v);
                        v.Parent = category;
                    }
                    return category;

                }
            }
            return indiagram;
        }
    }
}
