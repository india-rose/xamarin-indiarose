using System;
using System.Collections.Generic;
using Android.OS;

namespace IndiaRose.Droid.Helpers
{
	public static class BundleSave
	{
		private static readonly Dictionary<Guid, object> _savedItems = new Dictionary<Guid, object>();

		public static Guid Save<TObject>(TObject item)
		{
			Guid id = Guid.NewGuid();
			_savedItems.Add(id, item);
			return id;
		}

		public static TObject Get<TObject>(Guid id, TObject defaultValue = default(TObject))
		{
			if (_savedItems.ContainsKey(id))
			{
				object item = _savedItems[id];
				_savedItems.Remove(id);
				return (TObject) item;
			}
			return defaultValue;
		}

		public static Guid GetGuid(this Bundle bundle, string key)
		{
			return Guid.Parse(bundle.GetString(key));
		}

		public static void SetGuid(this Bundle bundle, string key, Guid id)
		{
			bundle.PutString(key, id.ToString());
		}
	}
}