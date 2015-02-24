using System.Collections.Generic;
using Storm.Mvvm;

namespace IndiaRose.Data.Model
{
	public class Indiagram : NotifierBase
	{
		private string _text;
		private string _imagePath;
		private string _soundPath;
		private Indiagram _parent;

		public string Text
		{
			get { return _text; }
			set { SetProperty(ref _text, value); }
		}

		public string ImagePath
		{
			get { return _imagePath; }
			set { SetProperty(ref _imagePath, value); }
		}

		public string SoundPath
		{
			get { return _soundPath; }
			set { SetProperty(ref _soundPath, value); }
		}

		public Indiagram Parent
		{
			get { return _parent; }
			set { SetProperty(ref _parent, value); }
		}

		public virtual bool IsCategory
		{
			get { return false; }
		}

		public virtual List<Indiagram> Children
		{
			get { return null; }
		}

		public Indiagram()
		{

		}

		public Indiagram(string text, string imagePath, string soundPath = null)
		{
			Text = text;
			ImagePath = imagePath;
			SoundPath = soundPath;
		}
	}
}
