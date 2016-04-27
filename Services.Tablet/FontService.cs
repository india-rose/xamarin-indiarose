using System.Collections.Generic;
using System.Globalization;
using IndiaRose.Interfaces;
using SharpDX.DirectWrite;

namespace IndiaRose.Services
{
	public class FontService : IFontService
	{
		private Dictionary<string, string> _font;

		public Dictionary<string, string> FontList
		{
			get { return _font ?? (_font = LoadFonts()); }
		}

        /// <summary>
        /// Charge la liste des polices du device
        /// </summary>
        /// <returns></returns>
		private Dictionary<string, string> LoadFonts()
		{
			Dictionary<string, string> result = new Dictionary<string, string>();
            //result.Add("toto", "toto");

			var factory = new Factory();
			var fontCollection = factory.GetSystemFontCollection(false);
			var familyCount = fontCollection.FontFamilyCount;

			for (int i = 0; i < familyCount; i++)
			{
				var fontFamily = fontCollection.GetFontFamily(i);
				var familyNames = fontFamily.FamilyNames;
				int index;

				if (!familyNames.FindLocaleName(CultureInfo.CurrentCulture.Name, out index))
				{
					familyNames.FindLocaleName("en-us", out index);
				}
				string name = familyNames.GetString(index);
				result.Add(name, name);
			}


			return result;
		}
	}
}
