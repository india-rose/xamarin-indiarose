using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm;

namespace IndiaRose.Services
{
	public abstract class AbstractSettingsService : NotifierBase, ISettingsService
	{
		#region Properties backing fields

		private uint _topBackgroundColor;
		private uint _bottomBackgroundColor;
		private int _selectionAreaHeight;
		private int _indiagramDisplaySize;
		private string _fontName;
		private int _fontSize;
		private bool _isReinforcerEnabled;
		private bool _isDragAndDropEnabled;
		private bool _isCategoryNameReadingEnabled;
		private bool _isBackHomeAfterSelectionEnabled;
		private float _timeOfSilenceBetweenWords;
		private uint _reinforcerColor;

		#endregion

		#region Properties
		
		public uint ReinforcerColor
		{
			get { return _reinforcerColor; }
			set { SetProperty(ref _reinforcerColor, value); }
		}

		public float TimeOfSilenceBetweenWords
		{
			get { return _timeOfSilenceBetweenWords; }
			set { SetProperty(ref _timeOfSilenceBetweenWords, value); }
		}

		public bool IsBackHomeAfterSelectionEnabled
		{
			get { return _isBackHomeAfterSelectionEnabled; }
			set { SetProperty(ref _isBackHomeAfterSelectionEnabled, value); }
		}

		public bool IsCategoryNameReadingEnabled
		{
			get { return _isCategoryNameReadingEnabled; }
			set { SetProperty(ref _isCategoryNameReadingEnabled, value); }
		}

		public bool IsDragAndDropEnabled
		{
			get { return _isDragAndDropEnabled; }
			set { SetProperty(ref _isDragAndDropEnabled, value); }
		}

		public bool IsReinforcerEnabled
		{
			get { return _isReinforcerEnabled; }
			set { SetProperty(ref _isReinforcerEnabled, value); }
		}

		public int FontSize
		{
			get { return _fontSize; }
			set { SetProperty(ref _fontSize, value); }
		}

		public string FontName
		{
			get { return _fontName; }
			set { SetProperty(ref _fontName, value); }
		}

		public int IndiagramDisplaySize
		{
			get { return _indiagramDisplaySize; }
			set { SetProperty(ref _indiagramDisplaySize, value); }
		}

		public int SelectionAreaHeight
		{
			get { return _selectionAreaHeight; }
			set { SetProperty(ref _selectionAreaHeight, value); }
		}

		public uint BottomBackgroundColor
		{
			get { return _bottomBackgroundColor; }
			set { SetProperty(ref _bottomBackgroundColor, value); }
		}

		public uint TopBackgroundColor
		{
			get { return _topBackgroundColor; }
			set { SetProperty(ref _topBackgroundColor, value); }
		}

		#endregion

		public void Save()
		{
			SettingsModel model = new SettingsModel
			{
				TopBackgroundColor = this.TopBackgroundColor,
				BottomBackgroundColor = this.BottomBackgroundColor,
				SelectionAreaHeight = this.SelectionAreaHeight,
				IndiagramDisplaySize = this.IndiagramDisplaySize,
				FontName = this.FontName,
				FontSize = this.FontSize,
				IsReinforcerEnabled = this.IsReinforcerEnabled,
				IsDragAndDropEnabled = this.IsDragAndDropEnabled,
				IsCategoryNameReadingEnabled = this.IsCategoryNameReadingEnabled,
				IsBackHomeAfterSelectionEnabled = this.IsBackHomeAfterSelectionEnabled,
				TimeOfSilenceBetweenWords = this.TimeOfSilenceBetweenWords,
				ReinforcerColor = this.ReinforcerColor
			};

			SaveOnDisk(model);
		}

		public void Load()
		{
			if (!ExistsOnDisk())
			{
				Reset();
				return;
			}

			SettingsModel model = LoadFromDisk();

			if (model == null)
			{
				Reset();
				return;
			}

			this.TopBackgroundColor = model.TopBackgroundColor;
			this.BottomBackgroundColor = model.BottomBackgroundColor;
			this.SelectionAreaHeight = model.SelectionAreaHeight;
			this.IndiagramDisplaySize = model.IndiagramDisplaySize;
			this.FontName = model.FontName;
			this.FontSize = model.FontSize;
			this.IsReinforcerEnabled = model.IsReinforcerEnabled;
			this.IsDragAndDropEnabled = model.IsDragAndDropEnabled;
			this.IsCategoryNameReadingEnabled = model.IsCategoryNameReadingEnabled;
			this.IsBackHomeAfterSelectionEnabled = model.IsBackHomeAfterSelectionEnabled;
			this.TimeOfSilenceBetweenWords = model.TimeOfSilenceBetweenWords;
			this.ReinforcerColor = model.ReinforcerColor;
		}

		public void Reset()
		{
			this.TopBackgroundColor = 0xFF3838FF;
			this.BottomBackgroundColor = 0xFF73739E;
			this.SelectionAreaHeight = 70;
			this.IndiagramDisplaySize = 128;
			this.FontName = "Consolas";
			this.FontSize = 12;
			this.IsReinforcerEnabled = true;
			this.IsDragAndDropEnabled = false;
			this.IsCategoryNameReadingEnabled = true;
			this.IsBackHomeAfterSelectionEnabled = true;
			this.TimeOfSilenceBetweenWords = 1.0f;
			this.ReinforcerColor = 0xFFFF00FF;
		}

		protected abstract bool ExistsOnDisk();
		protected abstract SettingsModel LoadFromDisk();
		protected abstract void SaveOnDisk(SettingsModel model);
	}
}
