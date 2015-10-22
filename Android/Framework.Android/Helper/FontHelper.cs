using System.Collections.Generic;
using Android.Graphics;
using Java.Lang;

namespace IndiaRose.Framework.Helper
{
    public static class FontHelper
    {
        private static readonly Dictionary<string, Typeface> _fonts = new Dictionary<string, Typeface>();

        public static Typeface LoadFont(string fontPath)
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