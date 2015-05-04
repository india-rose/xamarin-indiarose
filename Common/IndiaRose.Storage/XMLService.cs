using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using IndiaRose.Data.Model;
using SharpCompress.Archive;
using Storm.Mvvm.Inject;

namespace IndiaRose.Storage
{
    class XmlService : IXmlService
    {

        public void Initialize(Stream path)
        {
            var archive = ArchiveFactory.Open(path);

            List<Category> listCategories = new List<Category>();
            foreach (var t in archive.Entries)
            {
                var xd = XDocument.Load(t.OpenEntryStream());

                FillIndiagram(listCategories, xd, t.Key);
            }
        }

        private static void FillIndiagram(List<Category> listCategories, XDocument xd, string key)
        {

            XElement xe = xd.Element("indiagram");
            if (xd.FirstNode.ToString().StartsWith("<category>"))
            {
                xe = xd.Element("category");
            }

            Indiagram current = new Indiagram();
            if (key.Contains("/"))
            {
                string[] tab = key.Split('/');
                string parent = tab[tab.Length - 2];
                foreach (var t in listCategories)
                {
                    if (t.Text.Equals(parent))
                    {
                        current.Parent = t;
                    }
                }

            }

            foreach (var xNode in xe.Nodes())
            {
                var t = (XElement) xNode;
                if (t.ToString().StartsWith("<text>"))
                {
                    current.Text = t.Value;
                }
                if (t.ToString().StartsWith("<picture>"))
                {
                    current.ImagePath = t.Value;
                }
                if (t.ToString().StartsWith("<sound>"))
                {
                    current.SoundPath = t.Value;
                }
                if (t.ToString().StartsWith("<indiagrams>"))
                {
                    Category currentL = new Category();
	                currentL.CopyFrom(current);
                    listCategories.Add(currentL);
                    LazyResolver<ICollectionStorageService>.Service.Save(currentL);
                }
                else
                {
                    LazyResolver<ICollectionStorageService>.Service.Save(current);
                }
            }
        }
    }
}
