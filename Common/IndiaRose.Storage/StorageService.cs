using System.IO;
using System.Threading.Tasks;
using PCLStorage;

namespace IndiaRose.Storage
{
	public class StorageService : IStorageService
	{
		private const string STORAGE_DIRECTORY_NAME = "IndiaRose";
		private const string STORAGE_DATABASE_NAME = "india.sqlite";

		private readonly string _storageDirectory;

		public string DabatabasePath
		{
			get { return Path.Combine(RootPath, STORAGE_DATABASE_NAME); }
		}

		public string RootPath
		{
			get { return Path.Combine(_storageDirectory, STORAGE_DIRECTORY_NAME); }
		}

		public StorageService(string rootStorageDirectory)
		{
			_storageDirectory = rootStorageDirectory;
		}

		public async Task InitializeAsync()
		{
			IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync(_storageDirectory);

			if (await rootFolder.CheckExistsAsync(STORAGE_DIRECTORY_NAME) == ExistenceCheckResult.NotFound)
			{
				await rootFolder.CreateFolderAsync(STORAGE_DIRECTORY_NAME, CreationCollisionOption.FailIfExists);
			}
		}
	}
}
