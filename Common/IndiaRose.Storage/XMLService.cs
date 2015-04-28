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

            if (doc.FirstNode.ToString().Contains("category"))
            {
                Category c = new Category()
                {
                    Text = doc.Descendants("category").Descendants("text").First().Value,
                    ImagePath = doc.Descendants("category").Descendants("picture").First().Value,
                };

                return c;
            }
            else
            {
                Indiagram i = new Indiagram()
                {
                    Text = doc.Descendants("indiagram").Descendants("text").First().Value,
                    ImagePath = doc.Descendants("indiagram").Descendants("picture").First().Value,
                };

                return i;
            }
        }

        public void XmlToSql()
        {
            Task<List<IFile>> files = SearchXml();

        }
    }
}
