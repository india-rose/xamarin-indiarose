using System.Collections.Generic;
using System.IO;
using IndiaRose.Interfaces;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;

namespace IndiaRose.Services.Android
{
    public class FontService : AbstractServiceWithActivity, IFontService
    {
        public FontService(IActivityService activityService)
            : base(activityService)
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