using System;
using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit les méthodes et événement de gestion du TTS
    /// L'implémentation dépend de la plateforme
    /// </summary>
    public interface ITextToSpeechService
    {
        /// <summary>
        /// Event pour la fin de la lecture
        /// </summary>
	    event EventHandler SpeakingCompleted;
        /// <summary>
        /// Vrai si le TTS est en train de lire
        /// </summary>
	    bool IsReading { get; }
        /// <summary>
        /// Coupe la parole, libère les resources
        /// </summary>
	    void Close();
        /// <summary>
        /// Lit le texte de l'Indiagram passé en paramètre
        /// Lève l'event SpeakingCompleted lorsque la lecture est fini
        /// </summary>
        /// <param name="indiagram">L'Indiagram à lire</param>
        /// <seealso cref="SpeakingCompleted"/>
	    void PlayIndiagram(Indiagram indiagram);
    }
}
