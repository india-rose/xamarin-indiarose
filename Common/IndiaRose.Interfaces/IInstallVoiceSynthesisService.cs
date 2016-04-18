namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit les méthodes pour l'installation et la configuration des TTS
    /// L'implémentation dépend de la plateforme
    /// </summary>
    public interface IInstallVoiceSynthesisService
    {
        /// <summary>
        /// Redirige ou lance le téléchargement et l'installation d'un moteur de synthèse vocale
        /// </summary>
        void InstallVoiceSynthesisEngine();
        /// <summary>
        /// Redirige ou lance le téléchargement et l'installation de pack de langue pour la synthèse vocale
        /// </summary>
        void InstallLanguagePack();
        /// <summary>
        /// Redirige vers l'activation ou active la synthèse vocale précédemment téléchargé
        /// </summary>
        /// <see cref="InstallVoiceSynthesisEngine"/>
        void EnableVoiceSynthesisEngine();
    }
}
