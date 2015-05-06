using System.Collections.Generic;
using System.IO;
using IndiaRose.Interfaces;

namespace IndiaRose.Services.Android
{
	public class FontService : AbstractAndroidService, IFontService
	{
		private Dictionary<string, string> _fonts; 

        public Dictionary<string, string> FontList
        {
			get { return _fonts ?? (_fonts = LoadFonts()); }
        }

		private Dictionary<string, string> LoadFonts()
		{
			Dictionary<string, string> result = new Dictionary<string, string>();
			string[] files = Directory.GetFiles("/system/fonts", "*.ttf");
			foreach (string currentfile in files)
			{
				FileInfo fi = new FileInfo(currentfile);
				result.Add(fi.Name, currentfile);
			}
			return result;
		}
    }
}