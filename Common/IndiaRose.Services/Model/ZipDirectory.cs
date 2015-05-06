using System.Collections.Generic;
using SharpCompress.Archive;

namespace IndiaRose.Services.Model
{
	class ZipDirectory
	{
		public Dictionary<string, IArchiveEntry> Files { get; private set; }

		public Dictionary<string, ZipDirectory> Directories { get; private set; }

		public ZipDirectory()
		{
			Files = new Dictionary<string, IArchiveEntry>();
			Directories = new Dictionary<string, ZipDirectory>();
		}

		public static ZipDirectory FromArchive(IArchive archive)
		{
			ZipDirectory root = new ZipDirectory();

			Dictionary<string, ZipDirectory> directories = new Dictionary<string, ZipDirectory>
			{
				{"", root}
			};

			foreach (IArchiveEntry entry in archive.Entries)
			{
				string entryKey = entry.Key.Trim('/');

				string directoryPath = entryKey.Substring(0, entryKey.LastIndexOf('/') + 1);
				string entryName = entryKey.Substring(entryKey.LastIndexOf('/') + 1);

				ZipDirectory container = directories[directoryPath];

				if (entry.IsDirectory)
				{
					ZipDirectory newDirectory = new ZipDirectory();

					container.Directories.Add(entryName, newDirectory);
					directories.Add(entry.Key, newDirectory);
				}
				else
				{
					container.Files.Add(entryName, entry);
				}
			}

			return root;
		}
	}
}
