using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Enum décrivant les types de fichiers utilisables pour la fonction GenerateFilename
    /// </summary>
    /// <see cref="IStorageService.GenerateFilename"/>
	public enum StorageType
	{
		Sound,
		Image,
	}
    /// <summary>
    /// Interfaces pour les chemins d'accès aux resources importantes ainsi que les fonctions pour gérer les fichiers de l'application
    /// </summary>
	public interface IStorageService
	{
        /// <summary>
        /// Chemin du dossier des resources de l'application (bouton play, droite, etc)
        /// </summary>
		string AppPath { get; }

        /// <summary>
        /// Chemin du fichier de la BD
        /// </summary>
		string DatabasePath { get; }

        /// <summary>
        /// Chemin du dossier contenant le fichier des settings de l'application
        /// </summary>
		string SettingsFolderPath { get; }

        /// <summary>
        /// Nom du fichier des settings
        /// </summary>
		string SettingsFileName { get; }

        /// <summary>
        /// Chemin d'accès vers le fichier des settings
        /// </summary>
		string SettingsFilePath { get; }

        /// <summary>
        /// Chemin de l'application
        /// Racine de l'arborescence des fichiers IndiaRose
        /// </summary>
		string RootPath { get; }

        /// <summary>
        /// Chemin d'accès du dossier contenant les images des Indiagrams
        /// </summary>
        string ImagePath { get; }

        /// <summary>
        /// Chemin d'accès du dossier contenant les sons des Indiagrams
        /// </summary>
        string SoundPath { get; }

        /// <summary>
        /// Chemin d'accès pour l'image Home
        /// </summary>
        string ImageRootPath { get; }

        /// <summary>
        /// Chemin d'accès pour l'image Play
        /// </summary>
        string ImagePlayButtonPath { get; }

        /// <summary>
        /// Chemin d'accès pour l'image Correction
        /// </summary>
        string ImageCorrectionPath { get; }

        /// <summary>
        /// Chemin d'accès pour l'image Next
        /// </summary>
        string ImageNextArrowPath { get; }

        /// <summary>
        /// Chemin d'accès pour l'image Back
        /// </summary>
        string ImageBackPath { get; }

        /// <summary>
        /// Génère un nom unique et y ajoute l'extension voulu
        /// </summary>
        /// <param name="type">Type du fichier</param>
        /// <param name="extension">Extension voulu par le fichier</param>
        /// <returns>Le nom généré du fichier</returns>
        /// <see cref="StorageType"/>
	    string GenerateFilename(StorageType type, string extension);

        /// <summary>
        /// Vérifie de manière asynchrone l'existence des fichiers et dossiers nécessaire à India Rose les créant si besoin
        /// </summary>
        /// <returns>La tâche asynchrone représentant la vérification et la création</returns>
		Task InitializeAsync();

        /// <summary>
        /// Supprime les fichiers utilisateurs (images & sons des Indiagrams) non utilisés de l'application
        /// </summary>
	    void GarbageCollector();
	}
}
