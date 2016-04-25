using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Services.Model;
using PCLStorage;
using SharpCompress.Archive;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;

namespace IndiaRose.Services
{
    public class XmlService : IXmlService
    {
        private int _position;

        protected IStorageService StorageService => LazyResolver<IStorageService>.Service;

        protected ICollectionStorageService CollectionStorageService => LazyResolver<ICollectionStorageService>.Service;

        protected ISettingsService SettingService => LazyResolver<ISettingsService>.Service;

        #region Initialize collection from zip file

        public event EventHandler CollectionImported;

        public async Task InitializeCollectionFromZipStreamAsync(Stream zipStream)
        {
            IArchive archive = ArchiveFactory.Open(zipStream);

            string culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLowerInvariant();

            ZipDirectory zipRoot = ZipDirectory.FromArchive(archive);

            Dictionary<string, IArchiveEntry> resourceEntries = zipRoot.Files;

            ZipDirectory xmlDirectory = zipRoot.Directories.Single(x => x.Key == "xml").Value;
            if (xmlDirectory.Directories.All(x => x.Key != culture))
            {
                // load default language
                culture = "en";
            }

            ZipDirectory languageDirectory = xmlDirectory.Directories.Single(x => x.Key == culture).Value;

            foreach (IArchiveEntry entry in languageDirectory.Files.Values)
            {
                await ReadCollectionFromZip(entry, languageDirectory, "", resourceEntries, null);
            }

            this.RaiseEvent(CollectionImported);
        }

        private async Task ReadCollectionFromZip(IArchiveEntry entry, ZipDirectory currentDirectory, string currentPath, Dictionary<string, IArchiveEntry> resourceEntries, Indiagram parent)
        {
            using (Stream entryStream = entry.OpenEntryStream())
            {
                XDocument xmlDocument = XDocument.Load(entryStream);

                XElement rootElement = xmlDocument.Element("indiagram");
                if (rootElement != null)
                {
                    // the current element is an indiagram, just read it
                    await CreateIndiagramFromXml(rootElement, false, parent, async (key, type) =>
                    {
                        return await Task.Run(() =>
                        {
                            if (resourceEntries.ContainsKey(key))
                            {
                                return resourceEntries[key].OpenEntryStream();
                            }
                            throw new IndexOutOfRangeException(string.Format("Key {0} is not available in resources", key));
                        });
                    });
                }
                else
                {
                    // the current element is a category, read it + process its children
                    rootElement = xmlDocument.Element("category");
                    if (rootElement == null)
                    {
                        return;
                    }

                    Indiagram category = await CreateIndiagramFromXml(rootElement, true, parent, async (key, type) =>
                    {
                        return await Task.Run(() =>
                        {
                            if (resourceEntries.ContainsKey(key))
                            {
                                return resourceEntries[key].OpenEntryStream();
                            }
                            throw new IndexOutOfRangeException(string.Format("Key {0} is not available in resources", key));
                        });
                    });

                    XElement indiagramsElement = rootElement.Element("indiagrams");
                    if (indiagramsElement == null)
                    {
                        return;
                    }
                    foreach (XElement child in indiagramsElement.Elements("indiagram"))
                    {
                        // look for the entry
                        string directoryName = Path.GetDirectoryName(child.Value);
                        string fileName = Path.GetFileName(child.Value);

                        ZipDirectory directory = GetSubZipDirectory(currentDirectory, currentPath, directoryName);

                        if (directory.Files.ContainsKey(fileName))
                        {
                            await ReadCollectionFromZip(directory.Files[fileName], directory, directoryName, resourceEntries, category);
                        }
                        else
                        {
                            throw new IndexOutOfRangeException("file names mismatch");
                        }
                    }
                }
            }
        }

        private ZipDirectory GetSubZipDirectory(ZipDirectory current, string currentPath, string expectedPath)
        {
            if (string.Equals(currentPath, expectedPath, StringComparison.CurrentCultureIgnoreCase))
            {
                return current;
            }

            ZipDirectory sub = GetSubZipDirectory(current, currentPath, Path.GetDirectoryName(expectedPath));

            string dirName = Path.GetFileName(expectedPath);

            if (sub.Directories.ContainsKey(dirName))
            {
                return sub.Directories[dirName];
            }
            throw new IndexOutOfRangeException("directory names mismatch");
        }

        #endregion

        #region Initialize collection from old format

        public async Task<bool> HasOldCollectionFormatAsync()
        {
            // How to check ? just access the app directory and check for home.xml

            IFolder appFolder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.AppPath);
            return (await appFolder.CheckExistsAsync("home.xml") == ExistenceCheckResult.FileExists);
        }

        public async Task InitializeCollectionFromOldFormatAsync()
        {
            if (!await HasOldCollectionFormatAsync())
            {
                return;
            }

            IFolder appFolder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.AppPath);
            IFolder xmlFolder = await FileSystem.Current.GetFolderFromPathAsync(Path.Combine(StorageService.RootPath, "xml"));

            IFolder imageFolder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.ImagePath);
            IFolder soundFolder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.SoundPath);

            IFile homeFile = await appFolder.GetFileAsync("home.xml");

            Stream homeStream = await homeFile.OpenAsync(FileAccess.Read);
            using (homeStream)
            {
                XDocument xmlDocument = XDocument.Load(homeStream);

                XElement rootElement = xmlDocument.Element("category");
                if (rootElement == null)
                {
                    return;
                }
                XElement indiagramsElement = rootElement.Element("indiagrams");
                if (indiagramsElement == null)
                {
                    return;
                }
                foreach (XElement child in indiagramsElement.Elements("indiagram"))
                {
                    // look for the entry
                    string directoryName = Path.GetDirectoryName(child.Value);
                    string fileName = Path.GetFileName(child.Value);

                    IFolder folder = await GetSubFolderAsync(xmlFolder, "", directoryName);

                    if (await folder.CheckExistsAsync(fileName) == ExistenceCheckResult.FileExists)
                    {
                        IFile file = await folder.GetFileAsync(fileName);
                        Stream fileStream = await file.OpenAsync(FileAccess.Read);

                        using (fileStream)
                        {
                            await ReadCollectionFromOldFormat(fileStream, folder, directoryName, imageFolder, soundFolder, null);
                        }
                    }
                }
            }

            this.RaiseEvent(CollectionImported);
        }

        private async Task<IFolder> GetSubFolderAsync(IFolder directory, string currentPath, string expectedPath)
        {
            if (string.Equals(currentPath, expectedPath, StringComparison.CurrentCultureIgnoreCase))
            {
                return directory;
            }

            IFolder sub = await GetSubFolderAsync(directory, currentPath, Path.GetDirectoryName(expectedPath));
            string dirName = Path.GetFileName(expectedPath);

            if (await sub.CheckExistsAsync(dirName) == ExistenceCheckResult.FolderExists)
            {
                return await sub.GetFolderAsync(dirName);
            }
            throw new IndexOutOfRangeException("directory names mismatch");
        }

        private async Task<Stream> GetResourceStream(string resourcePath, IFolder resourceFolder)
        {
            IFolder folder = await GetSubFolderAsync(resourceFolder, "", Path.GetDirectoryName(resourcePath));
            string fileName = Path.GetFileName(resourcePath);
            if (await folder.CheckExistsAsync(fileName) == ExistenceCheckResult.FileExists)
            {
                IFile file = await folder.GetFileAsync(fileName);
                return await file.OpenAsync(FileAccess.Read);
            }
            throw new Exception(string.Format("Can not found resource file {0}", resourcePath));
        }

        private async Task ReadCollectionFromOldFormat(Stream inputStream, IFolder currentFolder, string currentDirectoryPath, IFolder imageFolder, IFolder soundFolder, Indiagram parent)
        {
            XDocument xmlDocument = XDocument.Load(inputStream);

            XElement rootElement = xmlDocument.Element("indiagram");
            if (rootElement != null)
            {
                // the current element is an indiagram, just read it
                await CreateIndiagramFromXml(rootElement, false, parent, ((key, type) => GetResourceStream(key, (type == StorageType.Image) ? imageFolder : soundFolder)));
            }
            else
            {
                // the current element is a category, read it + process its children
                rootElement = xmlDocument.Element("category");
                if (rootElement == null)
                {
                    return;
                }

                Indiagram category = await CreateIndiagramFromXml(rootElement, true, parent, ((key, type) => GetResourceStream(key, (type == StorageType.Image) ? imageFolder : soundFolder)));

                XElement indiagramsElement = rootElement.Element("indiagrams");
                if (indiagramsElement == null)
                {
                    return;
                }

                foreach (XElement child in indiagramsElement.Elements("indiagram"))
                {
                    // look for the entry
                    try
                    {
                        string directoryName = Path.GetDirectoryName(child.Value);
                        string fileName = Path.GetFileName(child.Value);

                        IFolder folder = await GetSubFolderAsync(currentFolder, currentDirectoryPath, directoryName);

                        if (await folder.CheckExistsAsync(fileName) == ExistenceCheckResult.FileExists)
                        {
                            IFile file = await folder.GetFileAsync(fileName);
                            Stream fileStream = await file.OpenAsync(FileAccess.Read);

                            using (fileStream)
                            {
                                await ReadCollectionFromOldFormat(fileStream, folder, directoryName, imageFolder, soundFolder, category);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //TODO : log error
                    }
                }
            }

        }

        #endregion

        #region Initialize Settings from old format 
        

        public async Task<bool> HasOldSettingsAsync()
        {
            IFolder appFolder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.AppPath);
            return (await appFolder.CheckExistsAsync(StorageService.OldSettingsFileName) == ExistenceCheckResult.FileExists);
        }

        public async Task ReadOldSettings()
        {
            if (!await HasOldSettingsAsync())
                return;

            IFolder appFolder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.AppPath);
            IFile settingFile = await appFolder.GetFileAsync(StorageService.OldSettingsFileName);
            Stream settingStream = await settingFile.OpenAsync(FileAccess.Read);
            using (settingStream)
            {
                XDocument xmlDocument = XDocument.Load(settingStream);
                XElement rootElement = xmlDocument.Element("settings");
                if (rootElement == null)
                    return;
                XElement heightSelection = rootElement.Element("heightSelectionArea");
                XElement backgroundSelection = rootElement.Element("backgroundSelectionArea");
                XElement reiforcerReadingColor = rootElement.Element("backgroundReinforcerReading");
                XElement sentenceArea = rootElement.Element("backgroundSentenceArea");
                XElement indiagramSize = rootElement.Element("indiagramSize");
                XElement fontFamily = rootElement.Element("fontFamily");
                XElement fontSize = rootElement.Element("fontSize");
                XElement wordsReadindDelay = rootElement.Element("wordsReadingDelay");
                XElement dragAndDrop = rootElement.Element("enableDragAndDrop");
                XElement categoryReading = rootElement.Element("enableCategoryReading");
                XElement readingReinforcer = rootElement.Element("enableReadingReinforcer");

                int num;
                if (int.TryParse(heightSelection?.Value, out num))
                    SettingService.SelectionAreaHeight = num;
                if (int.TryParse(indiagramSize?.Value, out num))
                    SettingService.IndiagramDisplaySize = num;
                if (int.TryParse(fontSize?.Value, out num))
                    SettingService.FontSize = num;
                SettingService.TimeOfSilenceBetweenWords = float.Parse(wordsReadindDelay?.Value, CultureInfo.InvariantCulture.NumberFormat);

                //Difference d'encodage
                //Voir si pas manière plus jolie pour le subString (enleve le # de la valeur)
                SettingService.TopBackgroundColor = "#FF" + backgroundSelection?.Value.ToUpper().Substring(1, 6);
                SettingService.ReinforcerColor = "#FF" + reiforcerReadingColor?.Value.ToUpper().Substring(1, 6);
                SettingService.BottomBackgroundColor = "#FF" + sentenceArea?.Value.ToUpper().Substring(1, 6);
                SettingService.FontName = fontFamily?.Value;

                SettingService.IsDragAndDropEnabled = dragAndDrop?.Value == "1";
                SettingService.IsCategoryNameReadingEnabled = categoryReading?.Value == "1"; ;
                SettingService.IsReinforcerEnabled = readingReinforcer?.Value == "1";

                SettingService.IsBackHomeAfterSelectionEnabled = true;
                SettingService.IsMultipleIndiagramSelectionEnabled = false;
                SettingService.TextColor = "#FFFFFFFF";

                await SettingService.SaveAsync();
            }

            //await settingFile.DeleteAsync();
        }


        #endregion

        #region Helper methods

        private async Task<Indiagram> CreateIndiagramFromXml(XElement rootElement, bool isCategory, Indiagram parent, Func<string, StorageType, Task<Stream>> resourceStreamOpener)
        {
            XElement textElement = rootElement.Element("text");
            XElement imagePathElement = rootElement.Element("picture");
            XElement soundPathElement = rootElement.Element("sound");

            if (textElement == null || imagePathElement == null || soundPathElement == null)
            {
                return null;
            }

            string text = textElement.Value;
            string imagePath = imagePathElement.Value;
            string soundPath = soundPathElement.Value;

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                imagePath = null;
            }

            if (string.IsNullOrWhiteSpace(soundPath))
            {
                soundPath = null;
            }

            //copy sound and imagePath if needed
            if (imagePath != null)
            {
                string localImagePath = StorageService.GenerateFilename(StorageType.Image, Path.GetExtension(imagePath));
                await CopyFileAsync(resourceStreamOpener(imagePath, StorageType.Image), localImagePath);

                imagePath = localImagePath;
            }

            if (soundPath != null)
            {
                string localSoundPath = StorageService.GenerateFilename(StorageType.Sound, Path.GetExtension(soundPath));
                await CopyFileAsync(resourceStreamOpener(soundPath, StorageType.Sound), localSoundPath);

                soundPath = localSoundPath;
            }

            Indiagram result = isCategory ? new Category() : new Indiagram();
            result.Text = text;
            result.ImagePath = imagePath;
            result.SoundPath = soundPath;
            result.Parent = parent;
            result.Position = _position++;
            result.IsEnabled = true;

            CollectionStorageService.Save(result);

            if (parent != null)
            {
                Category category = parent as Category;
                if (category != null)
                {
                    category.Children.Add(result);
                }
            }
            else
            {
                CollectionStorageService.Collection.Add(result);
            }

            return result;
        }

        private async Task CopyFileAsync(Task<Stream> inputStreamTask, string outputFilePath)
        {
            IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(Path.GetDirectoryName(outputFilePath));
            IFile outputFile = await folder.CreateFileAsync(Path.GetFileName(outputFilePath), CreationCollisionOption.ReplaceExisting);

            Stream outputStream = await outputFile.OpenAsync(FileAccess.ReadAndWrite);
            using (outputStream)
            {
                Stream inputStream = await inputStreamTask;
                using (inputStream)
                {
                    inputStream.CopyTo(outputStream);
                }
            }
        }

        #endregion
    }
}
