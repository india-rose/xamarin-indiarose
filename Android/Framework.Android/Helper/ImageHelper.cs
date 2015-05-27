//using System;
//using System.Collections.Generic;
//using Android.Graphics;

//namespace IndiaRose.Framework.Helper
//{
//	public static class ImageHelper
//	{
//		private static readonly Dictionary<String, Bitmap> _images = new Dictionary<String, Bitmap>();

//		public static Bitmap LoadImage(String imagePath, int width, int height)
//		{
//			String key = imagePath + "?" + width + "?" + height;
//			if (!_images.ContainsKey(key))
//			{
//				Bitmap image;

//				try
//				{
//					image = Bitmap.CreateScaledBitmap(BitmapFactory.DecodeFile(imagePath), width, height, true);
//				}
//				catch (Exception)
//				{
//					throw new Exception("ImageManager : Unable to load image " + imagePath);
//				}
//				_images.Add(key, image);
//			}

//			return _images[key];
//		}

//		public static Bitmap LoadImage(String imagePath)
//		{
//			String key = imagePath;
//			if (!_images.ContainsKey(key))
//			{
//				Bitmap image;
//				try
//				{
//					image = BitmapFactory.DecodeFile(imagePath);
//				}
//				catch (Exception)
//				{
//					throw new Exception("ImageManager : Unable to load image " + imagePath);
//				}

//				_images.Add(key, image);
//			}

//			return _images[key];
//		}
//	}
//}