namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit la taille de l'écran du device
    /// L'implémentation dépend de la plateforme
    /// </summary>
    public interface IScreenService
    {
        /// <summary>
        /// Largeur de l'écran
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Hauteur de l'écran
        /// </summary>
        int Height { get; }
    }
}
