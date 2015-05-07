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
		string AppPath { get; }

		string DatabasePath { get; }

		string SettingsFolderPath { get; }

		string SettingsFileName { get; }

		string SettingsFilePath { get; }

		string RootPath { get; }

        string ImagePath { get; }

        string SoundPath { get; }

	    string GenerateFilename(StorageType type, string extension);

		Task InitializeAsync();
	    void GarbageCollector();
	}
}
