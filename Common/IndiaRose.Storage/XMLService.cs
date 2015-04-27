using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IndiaRose.Data.Model;
using PCLStorage;


namespace IndiaRose.Storage
{
    class XmlService : IXmlService
    {
        private const string Path = ""; //dossier de destination à rajouter

        public async Task<List<IFile>> SearchXml()
        {
            IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(Path);
            List<IFile> xml = new List<IFile>();

            Dossiers(xml, folder);

            return xml;
        }

        public static async void Dossiers(List<IFile> list, IFolder path)
        {
            IList<IFolder> folders = await path.GetFoldersAsync();
            foreach (var t in folders)
            {
                Dossiers(list, t);
            }

            IList<IFile> files = await path.GetFilesAsync();
            list.AddRange(files);
        }
    }
}
