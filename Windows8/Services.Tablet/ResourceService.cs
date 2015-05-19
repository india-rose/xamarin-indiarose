using System;
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
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
	public class ResourceService : IResourceService
	{
		public async void ShowPdfFile(string pdfFileName)
		{
			// mupdf
		}

		public Stream OpenZip(string zipFileName)
		{
			throw new NotImplementedException();
		}

		private async void UnZipFile(string zipFileName)
		{
			var folder = ApplicationData.Current.LocalFolder;

			using (var zipStream = await folder.OpenStreamForReadAsync("zipFileName"))
			{
				using (MemoryStream zipMemoryStream = new MemoryStream((int)zipStream.Length))
				{
					await zipStream.CopyToAsync(zipMemoryStream);

					using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Read))
					{
						foreach (ZipArchiveEntry entry in archive.Entries)
						{
							if (entry.Name != "")
							{
								using (Stream fileData = entry.Open())
								{
									StorageFile outputFile = await folder.CreateFileAsync(entry.Name, CreationCollisionOption.ReplaceExisting);
									using (Stream outputFileStream = await outputFile.OpenStreamForWriteAsync())
									{
										await fileData.CopyToAsync(outputFileStream);
										await outputFileStream.FlushAsync();
									}
								}
							}
						}
					}
				}
			}
		}

		public async void Copy(string src, string dest)
		{
			var source = new Uri("ms-appx:///Assets/" + src);
			StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(source);
			FileIO.WriteTextAsync(file, dest);
		}
	}
}

