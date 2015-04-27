using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IndiaRose.Storage
{
    class XmlService : IXmlService
    {
        /*private const string Path = ""; //dossier de destination à rajouter

        public List<string> SearchXml()
        {
            string[] fichiers = Directory.GetDirectories(Path);
            List<string> xml = new List<string>();

            foreach (string t in fichiers)
            {
                Dossiers(ref xml, t);
            }

            fichiers = Directory.GetFiles(Path);

            xml.AddRange(fichiers);

            return xml;
        }

        public static void Dossiers(ref List<string> list, string path)
        {
            var fichiers = Directory.GetDirectories(path);

            foreach (string t in fichiers)
            {
                Dossiers(ref list, t);
            }

            fichiers = Directory.GetFiles(path);

            list.AddRange(fichiers);
        }*/
    }
}
