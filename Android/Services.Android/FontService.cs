using System.Collections.Generic;
using System.IO;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Services.Android
{
	public class FontService : AbstractAndroidService, IFontService
    {
        public FontService(IContainer container)
			: base(container)
        {
        }

        public Dictionary<string, string> FontList
        {
            get 
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
}