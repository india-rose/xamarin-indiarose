#region Usings 

using Storm.Mvvm;

#endregion

namespace IndiaRose.Data.Model
{
	public class Indiagram : NotifierBase
	{
		private int _id = -1;
		private string _imagePath;
		private bool _isEnabled = true;
		private Indiagram _parent;
		private int _position;
		private string _soundPath;
		private string _text;

		public int Id
		{
			get { return _id; }
			set { SetProperty(ref _id, value); }
		}

		public int Position
		{
			get { return _position; }
			set { SetProperty(ref _position, value); }
		}

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
		    set
		    {
		        SetProperty(ref _soundPath, value);
		        RaisePropertyChanged("HasCustomSound");
		    }
		}

		public Indiagram Parent
		{
			get { return _parent; }
			set { SetProperty(ref _parent, value); }
		}

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { SetProperty(ref _isEnabled, value); }
		}

		public virtual bool IsCategory
		{
			get { return false; }
		}

		public virtual bool HasChildren
		{
			get { return false; }
		}

		public bool HasCustomSound
		{
			get { return !string.IsNullOrWhiteSpace(SoundPath); }
		}

		public virtual void CopyFrom(Indiagram other, bool excludeId = false)
		{
			Text = other.Text;
			ImagePath = other.ImagePath;
			SoundPath = other.SoundPath;
			IsEnabled = other.IsEnabled;
			Position = other.Position;
			Parent = other.Parent;
			if (!excludeId)
			{
				Id = other.Id;
			}
		}

		public static bool AreSameIndiagram(Indiagram a, Indiagram b)
		{
			if (a == null && b == null)
			{
				return true;
			}

			if (a == null || b == null)
			{
				return false;
			}

			return a.Id == b.Id;
		}
	}
}