using System;
using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Contient les préférences utilisateurs et des fonctions pour les gérer
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Couleur de fond pour la partie utilisateur
        /// Couleur de la partie collection d'Indiagram (en haut
        /// </summary>
        string TopBackgroundColor { get; set; }

        /// <summary>
        /// Couleur de fond pour la partie utilisateur
        /// Couleur de la partie pour la phrase (en bas)
        /// </summary>
        string BottomBackgroundColor { get; set; }

        /// <summary>
        /// Couleur du renforçateur de lecture
        /// </summary>
        string ReinforcerColor { get; set; }

        /// <summary>
        /// Couleur de la police dans la partie utilisateur
        /// </summary>
        string TextColor { get; set; }

        /// <summary>
        /// Taille de la zone de sélection des Indiagrams dans la partie utilisateur (partie haute)
        /// </summary>
        int SelectionAreaHeight { get; set; }

        /// <summary>
        /// Taille des Indiagrams (à mettre au carré pour avoir la taille sur 2 dimensions)
        /// </summary>
        int IndiagramDisplaySize { get; set; }

        /// <summary>
        /// Nom de la police
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Taille de la police
        /// </summary>
        int FontSize { get; set; }

        /// <summary>
        /// Vrai si le renforçateur est activé
        /// </summary>
        bool IsReinforcerEnabled { get; set; }
        
        /// <summary>
        /// Vrai si le drag & drop est activé
        /// </summary>
        bool IsDragAndDropEnabled { get; set; }

        /// <summary>
        /// Vrai si lors de la navigation le TTS lit les catégories
        /// </summary>
        bool IsCategoryNameReadingEnabled { get; set; }

        /// <summary>
        /// Vrai si lors de la sélection d'un Indiagram on revient à la Home
        /// </summary>
        bool IsBackHomeAfterSelectionEnabled { get; set; }

        /// <summary>
        /// Vrai si on a la possibilité de sélectionner plusieurs fois un même Indiagram
        /// </summary>
		bool IsMultipleIndiagramSelectionEnabled { get; set; }

        /// <summary>
        /// Durée de silence entre 2 Indiagram lors de la lecture
        /// </summary>
        float TimeOfSilenceBetweenWords { get; set; }

        /// <summary>
        /// Sauvegarde de manière asynchrone les préférences
        /// </summary>
        /// <returns>La tâche asynchrone faisant la sauvegarde des préférences</returns>
        Task SaveAsync();

        /// <summary>
        /// Charge de manière asynchrone les préférences utilisateurs
        /// </summary>
        /// <returns>La tâche asynchrone exécutant le chargement des préférences</returns>
        Task LoadAsync();

        /// <summary>
        /// Remet les préférences utilisateurs à des valeurs par défaut
        /// </summary>
        void Reset();

        /// <summary>
        /// Vrai si toutes les préférences sont chargés
        /// Lève l'event Loaded lorsqu'il passe à vrai
        /// </summary>
        /// <see cref="Loaded"/>
        bool IsLoaded { get; set; }

        /// <summary>
        /// Événement indiquant la fin du chargement des préférences
        /// </summary>
        event EventHandler<EventArgs> Loaded;
    }
}