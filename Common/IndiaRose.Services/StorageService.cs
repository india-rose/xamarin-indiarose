#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using PCLStorage;
using Storm.Mvvm.Extensions;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Services
{
    public class StorageService : IStorageService, IInitializable
    {
        private const string STORAGE_DIRECTORY_NAME = "IndiaRose";
        private const string STORAGE_DATABASE_NAME = "india.sqlite";
        private const string STORAGE_IMAGE_NAME = "image";
        private const string STORAGE_SOUND_NAME = "sound";
        private const string STORAGE_SETTINGS_NAME = "settings.json";
        private const string STORAGE_APP_NAME = "app";
        private const string STORAGE_CORRECTION_IMAGE = "correction.png";
        private const string STORAGE_NEXTARROW_IMAGE = "nextarrow.png";
        private const string STORAGE_PLAYBUTTON_IMAGE = "playbutton.png";
        private const string STORAGE_ROOT_IMAGE = "root.png";
        private readonly string _storageDirectory;

        public string AppPath
        {
            get { return Path.Combine(RootPath, STORAGE_APP_NAME); }
        }

        public string ImageCorrectionPath
        {
            get { return Path.Combine(AppPath, STORAGE_CORRECTION_IMAGE); }
        }

        public string ImageRootPath
        {
            get { return Path.Combine(AppPath, STORAGE_ROOT_IMAGE); }
        }

        public string ImagePlayButtonPath
        {
            get { return Path.Combine(AppPath, STORAGE_PLAYBUTTON_IMAGE); }
        }

        public string ImageNextArrowPath
        {
            get { return Path.Combine(AppPath, STORAGE_NEXTARROW_IMAGE); }
        }

        public StorageService(string rootStorageDirectory)
        {
            _storageDirectory = rootStorageDirectory;
        }

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
            if (await indiaRoseFolder.CheckExistsAsync(STORAGE_APP_NAME) == ExistenceCheckResult.NotFound)
            {
                await indiaRoseFolder.CreateFolderAsync(STORAGE_APP_NAME, CreationCollisionOption.FailIfExists);
            }

            IFolder appFolder = await FileSystem.Current.GetFolderFromPathAsync(AppPath);
            var res = LazyResolver<IResourceService>.Service;
            if (await appFolder.CheckExistsAsync(STORAGE_CORRECTION_IMAGE) == ExistenceCheckResult.NotFound)
            {
                res.Copy(STORAGE_CORRECTION_IMAGE, ImageCorrectionPath);
            }
            if (await appFolder.CheckExistsAsync(STORAGE_PLAYBUTTON_IMAGE) == ExistenceCheckResult.NotFound)
            {
                res.Copy(STORAGE_PLAYBUTTON_IMAGE, ImagePlayButtonPath);
            }
            if (await appFolder.CheckExistsAsync(STORAGE_NEXTARROW_IMAGE) == ExistenceCheckResult.NotFound)
            {
                res.Copy(STORAGE_NEXTARROW_IMAGE, ImageNextArrowPath);
            }
            if (await appFolder.CheckExistsAsync(STORAGE_ROOT_IMAGE) == ExistenceCheckResult.NotFound)
            {
                res.Copy(STORAGE_ROOT_IMAGE, ImageRootPath);
            }
        }

        public string GenerateFilename(StorageType type, string extension)
        {
            switch (type)
            {
                case StorageType.Image:
                    return Path.Combine(ImagePath, string.Format("Image_{0}.{1}", Guid.NewGuid(), extension));
                case StorageType.Sound:
                    return Path.Combine(SoundPath, string.Format("Image_{0}.{1}", Guid.NewGuid(), extension));
            }
            throw new Exception("GenerateFilename : Type mismatch");
        }

        public async void GarbageCollector()
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