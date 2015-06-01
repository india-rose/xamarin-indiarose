//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Graphics.Imaging;
//using Windows.UI.Xaml.Media.Imaging;

//namespace IndiaRose.Framework.Helper
//{
//    class ImageHelper
//    {
//        private static readonly Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

//        public static BitmapImage LoadImage(string imagePath, int width, int height)
//        {
//            String key = imagePath + "?" + width + "?" + height;
//            if (!_images.ContainsKey(key))
//            {
//                BitmapImage image;

//                try
//                {
//                    image = BitmapImage.CreateScaledBitmap(BitmapFactory.DecodeFile(imagePath), width, height, true);
//                }
//                catch (Exception)
//                {
//                    throw new Exception("ImageManager : Unable to load image " + imagePath);
//                }
//                _images.Add(key, image);
//            }

//            return _images[key];
//        }

//        public static BitmapImage LoadImage(string imagePath)
//        {
//            String key = imagePath;
//            if (!_images.ContainsKey(key))
//            {
//                BitmapImage image;
//                try
//                {
//                    image = BitmapFactory.DecodeFile(imagePath);
//                }
//                catch (Exception)
//                {
//                    throw new Exception("ImageManager : Unable to load image " + imagePath);
//                }

//                _images.Add(key, image);
//            }

//            return _images[key];
//        }
//    }
//}
