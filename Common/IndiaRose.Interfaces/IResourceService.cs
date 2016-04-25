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
        /// <summary>
        /// Ouvre le stream des fichiers de l'archive pointé par le Path dans la string
        /// </summary>
        /// <param name="zipFileName">Le Path de l'archive à ouvrir</param>
        /// <returns>La tâche asynchrone de l'ouverture de l'archive</returns>
        Task<Stream> OpenZip(string zipFileName);
        /// <summary>
        /// Copie un fichier vers une nouvelle destination
        /// </summary>
        /// <param name="src">Le chemin du fichier à copier</param>
        /// <param name="dest">Le chemin où le fichier doit arriver</param>
        void Copy(string src,string dest);
    }
}
