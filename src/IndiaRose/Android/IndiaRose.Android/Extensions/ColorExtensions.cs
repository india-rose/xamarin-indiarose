using System;
using System.Globalization;
using Android.Graphics;

namespace IndiaRose.Droid.Extensions
{
	public static class ColorExtensions
	{
		public static Color ToColor(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return new Color(0);
			}

			string colorString = input.Trim('#');
			if (colorString.Length == 6 || colorString.Length == 8)
			{
				if (colorString.Length == 6)
				{
					colorString = $"FF{colorString}";
				}
				return uint.Parse(colorString, NumberStyles.HexNumber).ToColor();
			}
			return new Color(0);
		}

		public static Color ToColor(this uint input)
		{
			return new Color(
				(byte)(input >> 16 & 0xFF), 
				(byte)(input >> 8 & 0xFF), 
				(byte)(input >> 0 & 0xFF),
				(byte)(input >> 24 & 0xFF)
				);
		}

		public static int ToIntColor(this string input) => input.ToColor().ToArgb();

		public static string StringFromColor(this int input)
		{
			return $"#{input:X6}";
		}
	}
}