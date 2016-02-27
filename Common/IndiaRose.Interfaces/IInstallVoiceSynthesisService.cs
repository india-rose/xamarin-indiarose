namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit les méthodes pour l'installation et la configuration des TTS
    /// L'implémentation dépend de la plateforme
    /// </summary>
    public interface IInstallVoiceSynthesisService
    {
        void InstallVoiceSynthesisEngine();
        void InstallLanguagePack();
        void EnableVoiceSynthesisEngine();
    }
}
