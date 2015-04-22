using System;
using System.Collections.Generic;
using Android.Graphics;

namespace IndiaRose.Framework.Helper
{
    public static class FontHelper
    {
        private static readonly Dictionary<String, Typeface> _fonts = new Dictionary<String, Typeface>();

        public static Typeface LoadFont(String fontPath)
        {
            if (!_fonts.ContainsKey(fontPath))
            {
                Typeface font = Typeface.CreateFromFile(fontPath) ?? Typeface.Default;
                _fonts.Add(fontPath, font);
            }

            return _fonts[fontPath];
        }
    }
}