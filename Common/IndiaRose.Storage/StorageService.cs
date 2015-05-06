using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using PCLStorage;
using Storm.Mvvm.Extensions;
using Storm.Mvvm.Inject;

namespace IndiaRose.Storage
{
	public class StorageService : IStorageService
	{
		private const string STORAGE_DIRECTORY_NAME = "IndiaRose";
		private const string STORAGE_DATABASE_NAME = "india.sqlite";
		private const string STORAGE_IMAGE_NAME = "image";
		private const string STORAGE_SOUND_NAME = "sound";
		private const string STORAGE_SETTINGS_NAME = "settings.json";


		private readonly string _storageDirectory;

		public string DatabasePath
		{
			get { return Path.Combine(RootPath, STORAGE_DATABASE_NAME); }
		}

		public string SettingsFolderPath
		{
			get { return RootPath; }
		}

		public string SettingsFileName
		{
			get { return STORAGE_SETTINGS_NAME; }
		}

		public string SettingsFilePath
		{
			get { return Path.Combine(SettingsFolderPath, SettingsFileName); }
		}

		public string RootPath
		{
			get { return Path.Combine(_storageDirectory, STORAGE_DIRECTORY_NAME); }
		}

		public string ImagePath
		{
			get { return Path.Combine(RootPath, STORAGE_IMAGE_NAME); }
		}

		public string SoundPath
		{
			get { return Path.Combine(RootPath, STORAGE_SOUND_NAME); }
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

			IFolder indiaRoseFolder = await FileSystem.Current.GetFolderFromPathAsync(RootPath);
			if (await indiaRoseFolder.CheckExistsAsync(STORAGE_IMAGE_NAME) == ExistenceCheckResult.NotFound)
			{
				await indiaRoseFolder.CreateFolderAsync(STORAGE_IMAGE_NAME, CreationCollisionOption.FailIfExists);
			}
			if (await indiaRoseFolder.CheckExistsAsync(STORAGE_SOUND_NAME) == ExistenceCheckResult.NotFound)
			{
				await indiaRoseFolder.CreateFolderAsync(STORAGE_SOUND_NAME, CreationCollisionOption.FailIfExists);
			}
		}

		public string GenerationPath(string type, string extension)
		{
			switch (type)
			{
				case "image":
					return string.Format(ImagePath + "/Image_{0}.{1}", Guid.NewGuid(), extension);
				case "sound":
					return string.Format(SoundPath + "/Sound_{0}.{1}", Guid.NewGuid(), extension);
			}
			throw new Exception("GenerationPath : Type not match");
			//todo a voir avec julien
		}

	    public async void Garbage()
	    {
            //delete image
            IFolder imageFolder = await FileSystem.Current.GetFolderFromPathAsync(ImagePath);
            List<IFile> listFile = new List<IFile>(await imageFolder.GetFilesAsync());

            ObservableCollection<Indiagram> listIndia = LazyResolver<ICollectionStorageService>.Service.Collection;
            IEnumerable<string> listImagepath = listIndia.Select(x => x.ImagePath);

            listFile.RemoveAll(x => listImagepath.Contains(x.Path));
            listFile.ForEach(x => x.DeleteAsync());
            
            //delete sound
            IFolder soundFolder = await FileSystem.Current.GetFolderFromPathAsync(SoundPath);
            listFile = new List<IFile>(await soundFolder.GetFilesAsync());

            listIndia = LazyResolver<ICollectionStorageService>.Service.Collection;
            IEnumerable<string> listSoundpath = listIndia.Select(x => x.SoundPath);

            listFile.RemoveAll(x => listSoundpath.Contains(x.Path));
            listFile.ForEach(x => x.DeleteAsync());
	    }
	}
}
