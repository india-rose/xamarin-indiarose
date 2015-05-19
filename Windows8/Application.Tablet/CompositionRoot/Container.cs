using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Storage.Sqlite;
using PCLStorage;
using SQLite.Net.Platform.WinRT;
using Storm.Mvvm.Inject;

namespace IndiaRose.Application.CompositionRoot
{
	class Container : WindowsContainer
	{
		private StorageService _storageService;
		private ISettingsService _settingsService;
		private ICollectionStorageService _collectionStorageService;

		protected override void Initialize(Frame rootFrame, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			base.Initialize(rootFrame, views, dialogs);

			_storageService = new StorageService(FileSystem.Current.LocalStorage.Path);
			_settingsService = new SettingsService();
			_collectionStorageService = new SqliteCollectionStorageService(new SQLitePlatformWinRT());

			RegisterInstance<IEmailService>(new EmailService());
			//RegisterInstance<IResourceService>(new ResourceService());
			RegisterInstance<IEmailService>(new EmailService());
			//RegisterInstance<IInstallVoiceSynthesisService>(new InstallVoiceSynthesisService());
			RegisterInstance<IScreenService>(new ScreenService());
			RegisterInstance<IFontService>(new FontService());
            //RegisterInstance<IMediaService>(new MediaService());
            //RegisterInstance<IPopupService>(new PopupService());
            RegisterInstance<ICopyPasteService>(new CopyPasteService());
            //RegisterInstance<ITextToSpeechService>(new TextToSpeechService());

			RegisterInstance<IStorageService>(_storageService);
            RegisterInstance<ISettingsService>(_settingsService);
			RegisterInstance<IXmlService>(new XmlService());
			RegisterInstance<ICollectionStorageService>(_collectionStorageService);

			InitializeAsync();
		}

		protected async void InitializeAsync()
        {
            
			//await _storageService.InitializeAsync();
			await _settingsService.LoadAsync();
			//await _collectionStorageService.InitializeAsync();
            
		}
	}
}
