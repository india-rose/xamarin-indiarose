using System;
using System.Collections.Generic;
using Android.Graphics;

namespace IndiaRose.Framework.Helper
{
    /// <summary>
    /// Fournit des fonctions pour les images
    /// </summary>
    public static class ImageHelper
    {
        private static readonly Dictionary<string, Bitmap> _images = new Dictionary<string, Bitmap>();

        /// <summary>
        /// Charge une image Bitmap à partir d'un chemin d'accès
        /// </summary>
        /// <param name="imagePath">Le chemin vers l'image</param>
        /// <param name="width">Largeur dont l'image doit avoir</param>
        /// <param name="height">Hauteur dont l'image doit avoir</param>
        /// <returns>L'image sous forme de bitmap</returns>
        public static Bitmap LoadImage(string imagePath, int width, int height)
        {
            string key = imagePath + "?" + width + "?" + height;
            if (!_images.ContainsKey(key))
            {
                Bitmap image;

                try
                {
                    image = Bitmap.CreateScaledBitmap(BitmapFactory.DecodeFile(imagePath), width, height, true);
                }
                catch (Exception)
                {
					//TODO : log error
                    throw new Exception("ImageManager : Unable to load image " + imagePath);
                }
                _images.Add(key, image);
            }

            return _images[key];
        }

        /// <summary>
        /// Charge une iamge Bitmap à partir d'un chemin d'accès
        /// </summary>
        /// <param name="imagePath">Le chemin vers l'image</param>
        /// <returns>L'image sous forme de bitmap</returns>
        public static Bitmap LoadImage(string imagePath)
        {
            string key = imagePath;
            if (!_images.ContainsKey(key))
            {
                Bitmap image;
                try
                {
                    image = BitmapFactory.DecodeFile(imagePath);
                }
                catch (Exception)
                {
					//TODO : log error
                    throw new Exception("ImageManager : Unable to load image " + imagePath);
                }

                _images.Add(key, image);
            }

            return _images[key];
        }
    }
}