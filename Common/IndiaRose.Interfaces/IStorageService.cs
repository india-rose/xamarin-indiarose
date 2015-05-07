using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
	public enum StorageType
	{
		Sound,
		Image,
	}

	public interface IStorageService
	{
		string DatabasePath { get; }

		string SettingsFolderPath { get; }

		string SettingsFileName { get; }

		string SettingsFilePath { get; }

		string RootPath { get; }

        string ImagePath { get; }

        string SoundPath { get; }

        string AppPath { get; }

        string ImageRootPath { get; }

        string ImagePlayButtonPath { get; }

        string ImageCorrectionPath { get; }

        string ImageNextArrowPath { get; }

	    string GenerateFilename(StorageType type, string extension);

		Task InitializeAsync();
	    void GarbageCollector();
	}
}
