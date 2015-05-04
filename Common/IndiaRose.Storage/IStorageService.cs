using System.Threading.Tasks;

namespace IndiaRose.Storage
{
	public interface IStorageService
	{
		string DatabasePath { get; }

		string SettingsFolderPath { get; }

		string SettingsFileName { get; }

		string SettingsFilePath { get; }

		string RootPath { get; }

        string ImagePath { get; }

        string SoundPath { get; }

	    string GenerationPath(string type, string extension);

		Task InitializeAsync();
	}
}
