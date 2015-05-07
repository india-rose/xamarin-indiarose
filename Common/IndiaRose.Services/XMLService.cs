using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Services.Model;
using PCLStorage;
using SharpCompress.Archive;
using Storm.Mvvm.Inject;

namespace IndiaRose.Services
{
    public class XmlService : IXmlService
    {
	    private int _position = 0;

	    protected IStorageService StorageService
	    {
			get { return LazyResolver<IStorageService>.Service; }
	    }

	    protected ICollectionStorageService CollectionStorageService
	    {
			get { return LazyResolver<ICollectionStorageService>.Service; }
	    }

	    public void InitializeCollection(Stream zipStream)
	    {
			IArchive archive = ArchiveFactory.Open(zipStream);

		    string culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLowerInvariant();

		    ZipDirectory zipRoot = ZipDirectory.FromArchive(archive);

		    Dictionary<string, IArchiveEntry> imageEntries = zipRoot.Files;

		    ZipDirectory xmlDirectory = zipRoot.Directories.Single(x => x.Key == "xml").Value;
		    if (xmlDirectory.Directories.All(x => x.Key != culture))
		    {
				// load default language
			    culture = "en";
		    }

			ZipDirectory languageDirectory = xmlDirectory.Directories.Single(x => x.Key == culture).Value;

		    CreateCollection(languageDirectory, imageEntries);
	    }

	    private void CreateCollection(ZipDirectory rootXmlDirectory, Dictionary<string, IArchiveEntry> resourceEntries)
	    {
			foreach (IArchiveEntry entry in rootXmlDirectory.Files.Values)
		    {
			    ReadCollection(entry, rootXmlDirectory, "", resourceEntries, null);
		    }
	    }

	    private void ReadCollection(IArchiveEntry entry, ZipDirectory currentDirectory, string currentPath, Dictionary<string, IArchiveEntry> resourceEntries, Indiagram parent)
	    {
			using (Stream entryStream = entry.OpenEntryStream())
			{
				XDocument xmlDocument = XDocument.Load(entryStream);

				XElement rootElement = xmlDocument.Element("indiagram");
				if (rootElement != null)
				{
					// the current element is an indiagram, just read it
					CreateIndiagram(rootElement, false, parent, resourceEntries);
				}
				else
				{
					// the current element is a category, read it + process its children
					rootElement = xmlDocument.Element("category");
					Indiagram category = CreateIndiagram(rootElement, true, parent, resourceEntries);

					foreach (XElement child in rootElement.Element("indiagrams").Elements("indiagram"))
					{
						// look for the entry
						string directoryName = Path.GetDirectoryName(child.Value);
						string fileName = Path.GetFileName(child.Value);

						ZipDirectory directory = GetSubDirectory(currentDirectory, currentPath, directoryName);

						if (directory.Files.ContainsKey(fileName))
						{
							ReadCollection(directory.Files[fileName], directory, directoryName, resourceEntries, category);
						}
						else
						{
							throw new IndexOutOfRangeException("file names mismatch");
						}
					}
				}
			}
	    }

	    private ZipDirectory GetSubDirectory(ZipDirectory current, string currentPath, string expectedPath)
	    {
		    if (string.Equals(currentPath, expectedPath, StringComparison.CurrentCultureIgnoreCase))
		    {
			    return current;
		    }

		    ZipDirectory sub = GetSubDirectory(current, currentPath, Path.GetDirectoryName(expectedPath));

		    string dirName = Path.GetFileName(expectedPath);

		    if (sub.Directories.ContainsKey(dirName))
		    {
			    return sub.Directories[dirName];
		    }
		    throw new IndexOutOfRangeException("directory names mismatch");
	    }

		private Indiagram CreateIndiagram(XElement rootElement, bool isCategory, Indiagram parent, Dictionary<string, IArchiveEntry> resourceEntries)
	    {
			string text = rootElement.Element("text").Value;
			string imagePath = rootElement.Element("picture").Value;
		    string soundPath = rootElement.Element("sound").Value;

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
				CopyFileAsync(resourceEntries[imagePath], localImagePath);

			    imagePath = localImagePath;
		    }

			if (soundPath != null)
			{
				string localSoundPath = StorageService.GenerateFilename(StorageType.Sound, Path.GetExtension(soundPath));
				CopyFileAsync(resourceEntries[soundPath], localSoundPath);

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
				if (result is Category)
				{
					(result as Category).Children.Add(parent);
				}
			}
			else
			{
				CollectionStorageService.Collection.Add(result);
			}

		    return result;
	    }

	    private async void CopyFileAsync(IArchiveEntry inputFile, string outputFilePath)
	    {
		    IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(Path.GetDirectoryName(outputFilePath));
		    IFile outputFile = await folder.CreateFileAsync(Path.GetFileName(outputFilePath), CreationCollisionOption.ReplaceExisting);

		    Stream outputStream = await outputFile.OpenAsync(FileAccess.ReadAndWrite);
		    using (outputStream)
		    {
			    using (Stream inputStream = inputFile.OpenEntryStream())
			    {
					inputStream.CopyTo(outputStream);
			    }
		    }
	    }
    }
}
