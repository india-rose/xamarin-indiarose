
namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Interface pour l'affichage de popup
    /// L'implémentation dépend de la plateforme
    /// </summary>
    public interface IPopupService
    {
        /// <summary>
        /// Affiche un popup
        /// </summary>
        /// <param name="message">Le message à afficher</param>
        void DisplayPopup(string message);
    }
}
