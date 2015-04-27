using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PCLStorage;


namespace IndiaRose.Storage
{
    class XmlService : IXmlService
    {
        private const string Path = ""; //dossier de destination à rajouter

        public async Task<List<string>> SearchXml()
        {
            IFolder folders = await FileSystem.Current.GetFolderFromPathAsync(Path);
            IList<IFolder> listFolders = await folders.GetFoldersAsync();
            List<string> xml = new List<string>();

            foreach (IFolder t in listFolders)
            {
                Dossiers(xml, t);
            }

            IFile Files = await FileSystem.Current.GetFileFromPathAsync(Path);

            xml.AddRange(new[] {Files.Path});

            return xml;
        }

        public static async void Dossiers(List<string> list, IFolder path)
        {
            IFolder folders = await FileSystem.Current.GetFolderFromPathAsync(Path);
            IList<IFolder> listFolders = await folders.GetFoldersAsync();
            foreach (var t in listFolders)
            {
                Dossiers(list, t);
            }

            IFile files = await FileSystem.Current.GetFileFromPathAsync(Path);

            list.AddRange(new[] { files.Path });
        }

    }
}
