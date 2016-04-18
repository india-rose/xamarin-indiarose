using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit les méthodes pour copier coller des Indiagrams dans l'application
    /// </summary>
    public interface ICopyPasteService
    {
		bool HasBuffer { get; }
        /// <summary>
        /// Ajoute dans le buffer un Indiagram
        /// </summary>
        /// <param name="indiagram">L'Indiagram à mettre dans le buffer</param>
        /// <param name="isCategory">Spécifie si l'Indiagram est une catégorie</param>
        void Copy(Indiagram indiagram, bool isCategory);

        /// <summary>
        /// Rend l'Indiagram dans le buffer
        /// </summary>
        /// <returns>Indiagram contenu dans le buffer</returns>
        Indiagram Paste();
    }
}
