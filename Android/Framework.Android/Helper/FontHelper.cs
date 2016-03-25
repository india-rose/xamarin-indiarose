using System.Collections.Generic;
using Android.Graphics;
using Java.Lang;

namespace IndiaRose.Framework.Helper
{
    /// <summary>
    /// Fournit des fonctions pour les polices
    /// </summary>
    public static class FontHelper
    {
        private static readonly Dictionary<string, Typeface> _fonts = new Dictionary<string, Typeface>();

        /// <summary>
        /// Charge une police � partir de son chemin d'acc�s
        /// </summary>
        /// <param name="fontPath">Chemin d'acc�s � la police</param>
        /// <returns>L'objet de la police</returns>
        public static Typeface LoadFont(string fontPath)
        {
	        fontPath = fontPath ?? string.Empty;

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