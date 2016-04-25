#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using PCLCrypto;
using PCLStorage;
using Storm.Mvvm.Extensions;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

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
        private const string STORAGE_OLD_SETTINGS_NAME = "settings.xml";
        private const string STORAGE_APP_NAME = "app";
        private const string STORAGE_CORRECTION_IMAGE = "correction.png";
        private const string STORAGE_NEXTARROW_IMAGE = "nextarrow.png";
        private const string STORAGE_PLAYBUTTON_IMAGE = "playbutton.png";
        private const string STORAGE_ROOT_IMAGE = "root.png";
        private readonly string _storageDirectory;

        private IHashAlgorithmProvider _hasher;

        public string AppPath => Path.Combine(RootPath, STORAGE_APP_NAME);

        public string ImageCorrectionPath => Path.Combine(AppPath, STORAGE_CORRECTION_IMAGE);

        public string ImageRootPath => Path.Combine(AppPath, STORAGE_ROOT_IMAGE);

        public string ImagePlayButtonPath => Path.Combine(AppPath, STORAGE_PLAYBUTTON_IMAGE);

        public string ImageNextArrowPath => Path.Combine(AppPath, STORAGE_NEXTARROW_IMAGE);

        public StorageService(string rootStorageDirectory)
        {
            _storageDirectory = rootStorageDirectory;
        }

        public string DatabasePath => Path.Combine(RootPath, STORAGE_DATABASE_NAME);

        public string SettingsFolderPath => RootPath;

        public string SettingsFileName => STORAGE_SETTINGS_NAME;

        public string OldSettingsFileName => STORAGE_OLD_SETTINGS_NAME;

        public string SettingsFilePath => Path.Combine(SettingsFolderPath, SettingsFileName);

        public string OldSettingsFilePath => Path.Combine(AppPath, OldSettingsFileName);

        public string RootPath => Path.Combine(_storageDirectory, STORAGE_DIRECTORY_NAME);

        public string ImagePath => Path.Combine(RootPath, STORAGE_IMAGE_NAME);

        public string SoundPath => Path.Combine(RootPath, STORAGE_SOUND_NAME);

        public async Task InitializeAsync()
        {
            _hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);

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

            CheckAssetsAsync();
        }

        public string GenerateFilename(StorageType type, string extension)
        {
            switch (type)
            {
                case StorageType.Image:
                    return Path.Combine(ImagePath, string.Format("Image_{0}.{1}", Guid.NewGuid(), extension));
                case StorageType.Sound:
                    return Path.Combine(SoundPath, string.Format("Sound_{0}.{1}", Guid.NewGuid(), extension));
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

        #region Ajouts Martin pour correction https://github.com/india-rose/xamarin-indiarose/issues/2
        /* Permet de changer les assets lors du passage à la dernière version
         * On compare donc les fichiers présents dans le dossier IndiaRose/app et avec les assets
         * Pour chaque fichier, on récupère le stream correspondant, qu'on lie, puis qu'on hash en SHA1
         * Un Hase a une taille fixe.
         * Si les hash sont différents, c'est que les fichiers le sont aussi, il faut donc copier le fichier,
         * des assets vers le dossier app.
         * */
        private async void CheckAssetsAsync()
        {
            IFolder appFolder = await FileSystem.Current.GetFolderFromPathAsync(AppPath);

            IList<IFile> fileList = await appFolder.GetFilesAsync();
            foreach (var file in fileList)
            {
                if (file.Name != STORAGE_CORRECTION_IMAGE && file.Name != STORAGE_PLAYBUTTON_IMAGE &&
                    file.Name != STORAGE_NEXTARROW_IMAGE && file.Name != STORAGE_ROOT_IMAGE)
                    continue;

                // File
                Stream streamFile = await file.OpenAsync(FileAccess.Read);
                string contentFile = ReadStream(streamFile);
                string hashFile = Hash(contentFile);
                streamFile.Dispose();

                // Asset
                Stream streamAsset = LazyResolver<IAssetsService>.Service.OpenAssets(file.Name);
                string contentAsset = ReadStream(streamAsset);
                string hashAsset = Hash(contentAsset);
                streamAsset.Dispose();

                if (hashFile != hashAsset)
                {
                    if (file.Name == STORAGE_CORRECTION_IMAGE)
                    {
                        LazyResolver<IResourceService>.Service.Copy(STORAGE_CORRECTION_IMAGE, ImageCorrectionPath);
                    }
                    else if (file.Name == STORAGE_PLAYBUTTON_IMAGE)
                    {
                        LazyResolver<IResourceService>.Service.Copy(STORAGE_PLAYBUTTON_IMAGE, ImagePlayButtonPath);
                    }
                    else if (file.Name == STORAGE_NEXTARROW_IMAGE)
                    {
                        LazyResolver<IResourceService>.Service.Copy(STORAGE_NEXTARROW_IMAGE, ImageNextArrowPath);
                    }
                    else if (file.Name == STORAGE_ROOT_IMAGE)
                    {
                        LazyResolver<IResourceService>.Service.Copy(STORAGE_ROOT_IMAGE, ImageRootPath);
                    }
                }
            }
        }

        // Prend une string, la hash en SHA1, puis renvoie le hash sous forme de chaine
        private string Hash(string text)
        {
            byte[] input = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] output = _hasher.HashData(input);
            string res = BitConverter.ToString(output);
            return res;
        }

        // Lit un stream en entier et renvoie son contenu dans une string
        private string ReadStream(Stream stream)
        {
            byte[] buffer = new byte[2048];
            string str = "";
            while (stream.Read(buffer, 0, buffer.Length) > 0)
            {
                str += BitConverter.ToString(buffer);
                str = str.Replace("-", "");
            }
            return str;
        }
        #endregion
    }
}