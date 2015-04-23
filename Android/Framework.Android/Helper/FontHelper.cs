using System.Collections.Generic;
using Android.Graphics;
using Java.Lang;
using String = System.String;

namespace IndiaRose.Framework.Helper
{
    public static class FontHelper
    {
        private static readonly Dictionary<String, Typeface> _fonts = new Dictionary<String, Typeface>();

        public static Typeface LoadFont(String fontPath)
        {
            if (!_fonts.ContainsKey(fontPath))
            {
                Typeface font;
                try
                {
                    font = Typeface.CreateFromFile(fontPath);
                }
                catch (RuntimeException)
                {
                    font = Typeface.Default;
                }
                _fonts.Add(fontPath, font);
            }

            return _fonts[fontPath];
        }
    }
}