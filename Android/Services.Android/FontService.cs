using System;
using System.Collections.Generic;
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
                string[] files = System.IO.Directory.GetFiles("/system/fonts", "*.ttf");
                foreach (string currentfile in files)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(currentfile);
                    result.Add(fi.Name, currentfile);
                }
                return result;
            }
        }
    }
}