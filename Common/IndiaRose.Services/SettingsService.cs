using System;
using System.ComponentModel;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Newtonsoft.Json;
using PCLStorage;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
	public class SettingsService : NotifierBase, ISettingsService
	{
		private bool _hasChanged;

		#region Services

		protected ILoggerService LoggerService
		{
			get { return LazyResolver<ILoggerService>.Service; }
		}

		protected IStorageService StorageService
		{
			get { return LazyResolver<IStorageService>.Service; }
		}

		#endregion

		#region Properties backing fields

		private string _topBackgroundColor;
		private string _bottomBackgroundColor;
		private int _selectionAreaHeight;
		private int _indiagramDisplaySize;
		private string _fontName;
		private int _fontSize;
		private bool _isReinforcerEnabled;
		private bool _isDragAndDropEnabled;
		private bool _isCategoryNameReadingEnabled;
		private bool _isBackHomeAfterSelectionEnabled;
		private bool _isMultipleIndiagramSelectionEnabled;
        private float _timeOfSilenceBetweenWords;
        private string _reinforcerColor;
        private string _textColor;

	    private bool _isLoaded;

		#endregion

		#region Properties

        public string ReinforcerColor
        {
            get { return _reinforcerColor; }
            set { SetProperty(ref _reinforcerColor, value); }
        }
        public string TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

		public float TimeOfSilenceBetweenWords
		{
			get { return _timeOfSilenceBetweenWords; }
			set { SetProperty(ref _timeOfSilenceBetweenWords, value); }
		}

		public bool IsMultipleIndiagramSelectionEnabled
		{
			get { return _isMultipleIndiagramSelectionEnabled; }
			set { SetProperty(ref _isMultipleIndiagramSelectionEnabled, value); }
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

		public string BottomBackgroundColor
		{
			get { return _bottomBackgroundColor; }
			set { SetProperty(ref _bottomBackgroundColor, value); }
		}

		public string TopBackgroundColor
		{
			get { return _topBackgroundColor; }
			set { SetProperty(ref _topBackgroundColor, value); }
		}


        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                SetProperty(ref _isLoaded, value);
                if (Loaded != null) Loaded(this,new EventArgs());
            }
        }
		#endregion

		public SettingsService()
		{
			PropertyChanged += OnAnyValueChanged;
		}

		private void OnAnyValueChanged(object sender, PropertyChangedEventArgs e)
		{
			_hasChanged = true;
		}

		public async Task SaveAsync()
		{
			if(!_hasChanged)
			{ 
				return;
			}

			SettingsModel model = new SettingsModel
			{
				TopBackgroundColor = TopBackgroundColor,
				BottomBackgroundColor = BottomBackgroundColor,
				SelectionAreaHeight = SelectionAreaHeight,
				IndiagramDisplaySize = IndiagramDisplaySize,
				FontName = FontName,
				FontSize = FontSize,
				IsReinforcerEnabled = IsReinforcerEnabled,
				IsDragAndDropEnabled = IsDragAndDropEnabled,
				IsCategoryNameReadingEnabled = IsCategoryNameReadingEnabled,
				IsBackHomeAfterSelectionEnabled = IsBackHomeAfterSelectionEnabled,
				IsMultipleIndiagramSelectionEnabled = IsMultipleIndiagramSelectionEnabled,
				TimeOfSilenceBetweenWords = TimeOfSilenceBetweenWords,
				ReinforcerColor = ReinforcerColor,
                TextColor = TextColor
			};

			await SaveOnDiskAsync(model);
		}

		public async Task LoadAsync()
		{
			if (!await ExistsOnDiskAsync())
			{
                Reset();
                IsLoaded = true;
				return;
			}

			SettingsModel model = await LoadFromDiskAsync();

			if (model == null)
			{
                Reset();
                IsLoaded = true;
				return;
			}

			TopBackgroundColor = model.TopBackgroundColor;
			BottomBackgroundColor = model.BottomBackgroundColor;
			SelectionAreaHeight = model.SelectionAreaHeight;
			IndiagramDisplaySize = model.IndiagramDisplaySize;
			FontName = model.FontName;
			FontSize = model.FontSize;
			IsReinforcerEnabled = model.IsReinforcerEnabled;
			IsDragAndDropEnabled = model.IsDragAndDropEnabled;
			IsCategoryNameReadingEnabled = model.IsCategoryNameReadingEnabled;
			IsBackHomeAfterSelectionEnabled = model.IsBackHomeAfterSelectionEnabled;
			IsMultipleIndiagramSelectionEnabled = model.IsMultipleIndiagramSelectionEnabled;
			TimeOfSilenceBetweenWords = model.TimeOfSilenceBetweenWords;
			ReinforcerColor = model.ReinforcerColor;
		    TextColor = model.TextColor;

		    IsLoaded = true;

			_hasChanged = false;
		}

		public void Reset()
		{
			TopBackgroundColor = "#FF3838FF";
			BottomBackgroundColor = "#FF73739E";
			SelectionAreaHeight = 50;
			IndiagramDisplaySize = 128;
			FontName = "Consolas";
			FontSize = 20;
			IsReinforcerEnabled = true;
			IsDragAndDropEnabled = false;
			IsCategoryNameReadingEnabled = true;
			IsBackHomeAfterSelectionEnabled = true;
			IsMultipleIndiagramSelectionEnabled = false;
			TimeOfSilenceBetweenWords = 1.0f;
			ReinforcerColor = "#FFFF00FF";
		    TextColor = "#FFFFFFFF";
		}

	    public event EventHandler<EventArgs> Loaded;

	    protected async Task<bool> ExistsOnDiskAsync()
		{
			try
			{
				IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.SettingsFolderPath);
				ExistenceCheckResult result = await folder.CheckExistsAsync(StorageService.SettingsFileName);
				return result == ExistenceCheckResult.FileExists;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		protected async Task<SettingsModel> LoadFromDiskAsync()
		{
			try
			{
				IFile file = await FileSystem.Current.GetFileFromPathAsync(StorageService.SettingsFilePath);
				string content = await file.ReadAllTextAsync();
				SettingsModel result = JsonConvert.DeserializeObject<SettingsModel>(content);
				return result;
			}
			catch (Exception e)
			{
				LoggerService.Log("IndiaRose.Services.SettingsService.LoadFromDiskAsync() : exception while trying to load content from the settings file " + e, MessageSeverity.Critical);
				return null;
			}

		}

		protected async Task SaveOnDiskAsync(SettingsModel model)
		{
			try
			{
				IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(StorageService.SettingsFolderPath);
				IFile file = await folder.CreateFileAsync(StorageService.SettingsFileName, CreationCollisionOption.ReplaceExisting);
				string content = JsonConvert.SerializeObject(model, Formatting.Indented);
				await file.WriteAllTextAsync(content);
			}
			catch (Exception e)
			{
				LoggerService.Log("IndiaRose.Services.SettingsService.SaveOnDiskAsync() : exception while trying to write content to the settings file " + e, MessageSeverity.Critical);
			}
		}
	}
}
