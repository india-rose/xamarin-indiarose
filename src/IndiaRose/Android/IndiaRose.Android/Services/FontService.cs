using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IndiaRose.Core.Interfaces;

namespace IndiaRose.Droid.Services
{
	public class FontService : IFontService
	{
		private Dictionary<string, string> _fonts;

		public async Task<List<string>> GetFontDisplayNames()
		{
			return (await LoadFonts()).Keys.ToList();
		}

		public async Task<string> GetFontFamilyForDisplayName(string name)
		{
			Dictionary<string, string> fonts = await LoadFonts();

			return fonts.ContainsKey(name) ? fonts[name] : null;
		}

		private Task<Dictionary<string, string>> LoadFonts()
		{
			if (_fonts != null)
			{
				return Task.FromResult(_fonts);
			}
			return Task.Run(() =>
			{
				_fonts = Directory.GetFiles("/system/fonts", "*.ttf").GroupBy(filePath =>
				{
					string fileName = Path.GetFileNameWithoutExtension(filePath) ?? "";
					int dashIndex = fileName.IndexOf('-');
					if (dashIndex >= 0)
					{
						return fileName.Substring(0, dashIndex);
					}
					return fileName;
				}).ToDictionary(group => group.Key, group => group.FirstOrDefault(x => x.IndexOf('-') < 0) ?? group.FirstOrDefault(x => x.EndsWith("Regular.ttf", StringComparison.InvariantCultureIgnoreCase)) ?? group.FirstOrDefault());
				return _fonts;
			});
		}
	}
}