using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using IndiaRose.Framework;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
	public class ResourceService : IResourceService
	{
		public async void ShowPdfFile(string pdfFileName)
		{
			var source = new Uri("ms-appx:///Assets/" + pdfFileName);
			StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(source);
			Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(pdfFileName);
			Windows.System.Launcher.LaunchFileAsync(file);
		}

		public async Task<Stream> OpenZip(string zipFileName)
		{
			var source = new Uri("ms-appx:///Assets/" + zipFileName);
			StorageFolder installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
			StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(source);
			IRandomAccessStream tmp = await file.OpenAsync(FileAccessMode.Read);
			return tmp.AsStream();
		}
		public async void Copy(string src, string dest)
		{
			var source = new Uri("ms-appx:///Assets/" + src);
			StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(source);
		    StorageFolder fold = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(dest));
		    await file.CopyAsync(fold, file.Name);
		}
	}
}

