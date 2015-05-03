#region Usings 

using System;
using Storm.Mvvm;

#endregion

namespace IndiaRose.Data.Model
{
	public class Indiagram : NotifierBase
	{
		private int _id;
		private string _imagePath;
		private bool _isEnabled;
		private Category _parent;
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
			set { SetProperty(ref _soundPath, value); }
		}

		public Category Parent
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

		public Indiagram()
		{
			IsEnabled = true;
		}

		public Indiagram(int id)
		{
			_id = id;
		}

		public Indiagram(string text, string imagePath, string soundPath = null)
		{
			Text = text;
			ImagePath = imagePath;
			SoundPath = soundPath;
			IsEnabled = true;
		}

		public Indiagram(Indiagram cloneIndiagram, bool updateparent = false)
		{
			Edit(cloneIndiagram, updateparent);
		}

		public Indiagram(string text, string imagePath, Category a)
		{
			Parent = a;
			a.Children.Add(this);
			Text = text;
			ImagePath = imagePath;
			IsEnabled = true;
		}

		public void Edit(Indiagram cloneIndiagram, bool updateparent = false)
		{
			Position = cloneIndiagram.Position;
			Text = cloneIndiagram.Text;
			ImagePath = cloneIndiagram.ImagePath;
			SoundPath = cloneIndiagram.SoundPath;
			IsEnabled = cloneIndiagram.IsEnabled;
			if (updateparent)
			{
				if (!AreSameIndiagram(Parent, cloneIndiagram.Parent))
				{
					try
					{
						Parent.Children.Remove(this);
					}
					catch (NullReferenceException)
					{
						//Indiagram do not have parent, then don't remove it from its children's list
					}
					Parent = cloneIndiagram.Parent;
					Parent.Children.Add(this);
				}
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