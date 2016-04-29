using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using IndiaRose.Storage.Sqlite;
using PCLStorage;
using Services.Tablet;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using SQLite.Net.Platform.WinRT;

namespace Application.Tablet.CompositionRoot
{
	class Container : WindowsContainer
	{
        private ISettingsService _settingsService;
        private StorageService _storageService;
		private ICollectionStorageService _collectionStorageService;
	    private IInitializationStateService _initializationStateService;

        protected override void Initialize(Frame rootFrame, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			base.Initialize(rootFrame, views, dialogs);

            _settingsService = new SettingsService();
            _storageService = new StorageService(FileSystem.Current.LocalStorage.Path);
		    _collectionStorageService = new SqliteCollectionStorageService(new SQLitePlatformWinRT());
            _initializationStateService = new InitializationStateService(2);

            RegisterInstance<INavigationService>(new NavigationService(rootFrame,views));
            RegisterInstance<ISettingsService>(_settingsService);
            RegisterInstance<IScreenService>(new ScreenService());
            RegisterInstance<IFontService>(new FontService());
            RegisterInstance<IEmailService>(new EmailService());
			RegisterInstance<IResourceService>(new ResourceService());
			RegisterInstance<IEmailService>(new EmailService());
            RegisterInstance<IMediaService>(new MediaService());
            RegisterInstance<IPopupService>(new PopupService());
            RegisterInstance<ICopyPasteService>(new CopyPasteService());
            RegisterInstance<ITextToSpeechService>(new TextToSpeechService());
            RegisterInstance<IInitializationStateService>(_initializationStateService);

			RegisterInstance<IStorageService>(_storageService);
			RegisterInstance<IXmlService>(new XmlService());
			RegisterInstance<ICollectionStorageService>(_collectionStorageService);

            InitializeAsync();
        }

        protected async void InitializeAsync()
        {
            await _storageService.InitializeAsync();
			await _settingsService.LoadAsync();
            await _collectionStorageService.InitializeAsync();
		}
	}
}
