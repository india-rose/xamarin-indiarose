﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

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
			FileIO.WriteTextAsync(file, dest);
		}
	}
}

