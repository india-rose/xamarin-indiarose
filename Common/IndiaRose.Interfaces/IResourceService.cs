using System.IO;
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit des méthodes pour utiliser les fichiers dans le système
    /// Les implémentations dépendent de la plateforme
    /// </summary>
    public interface IResourceService
    {
        /// <summary>
        /// Affiche un fichier pdf
        /// </summary>
        /// <param name="pdfFileName">Le chemin du pdf à ouvrir</param>
        void ShowPdfFile(string pdfFileName);
        //TODO
        Task<Stream> OpenZip(string zipFileName);
        /// <summary>
        /// Copie un fichier vers une nouvelle destination
        /// </summary>
        /// <param name="src">Le chemin du fichier à copier</param>
        /// <param name="dest">Le chemin où le fichier doit arriver</param>
        void Copy(string src,string dest);
    }
}
