using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using PCLStorage;
using Services.Tablet;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
//using SQLite.Net.Platform.WinRT;

namespace Application.Tablet.CompositionRoot
{
	class Container : WindowsContainer
	{
        private ISettingsService _settingsService;
        private StorageService _storageService;
		private ICollectionStorageService _collectionStorageService;

        protected override void Initialize(Frame rootFrame, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			base.Initialize(rootFrame, views, dialogs);
            _settingsService = new SettingsService();

            _storageService = new StorageService(FileSystem.Current.LocalStorage.Path);
		   // _collectionStorageService = new SqliteCollectionStorageService(new SQLitePlatformWinRT());*/

            RegisterInstance<INavigationService>(new NavigationService(rootFrame,views));
            RegisterInstance<ISettingsService>(_settingsService);
            RegisterInstance<IScreenService>(new ScreenService());
            RegisterInstance<IFontService>(new FontService());
            RegisterInstance<IEmailService>(new EmailService());
			RegisterInstance<IResourceService>(new ResourceService());
			RegisterInstance<IEmailService>(new EmailService());
			//RegisterInstance<IInstallVoiceSynthesisService>(new InstallVoiceSynthesisService());
            //RegisterInstance<IMediaService>(new MediaService());
            //RegisterInstance<IPopupService>(new PopupService());
            RegisterInstance<ICopyPasteService>(new CopyPasteService());
            //RegisterInstance<ITextToSpeechService>(new TextToSpeechService());

			RegisterInstance<IStorageService>(_storageService);
			RegisterInstance<IXmlService>(new XmlService());
			RegisterInstance<ICollectionStorageService>(_collectionStorageService);

            InitializeAsync();
        }

		protected async void InitializeAsync()
        {

            await _storageService.InitializeAsync();
			await _settingsService.LoadAsync();
            //await _collectionStorageService.InitializeAsync();
            
		}
	}
}
