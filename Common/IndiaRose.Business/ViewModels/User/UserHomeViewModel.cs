using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.User
{
    public class UserHomeViewModel : AbstractBrowserViewModel
    {

        private readonly object _lockMutex = new object();
        private bool _initialized = false;

        protected IXmlService XmlService
        {
            get { return LazyResolver<IXmlService>.Service; }
        }
        protected IResourceService ResourceService
        {
            get { return LazyResolver<IResourceService>.Service; }
        }
        protected ITextToSpeechService TtsService
        {
            get { return LazyResolver<ITextToSpeechService>.Service; }
        }
        protected IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }
        private List<Indiagram> _toplayList; 
        public string BotBackgroundColor
        {
            get { return SettingsService.BottomBackgroundColor; }
        }

        public List<Indiagram> ToPlayedList
        {
            get { return _toplayList; }
            set { SetProperty(ref _toplayList, value); }
        }

        public UserHomeViewModel()
        {
            ToPlayedList=new List<Indiagram>();
        }
        public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
        {
            base.OnNavigatedTo(e, parametersKey);

            lock (_lockMutex)
            {
                CollectionStorageService.Initialized += (sender, args) =>
                {
                    lock (_lockMutex)
                    {
                        OnCollectionInitialized();
                    }
                };
                if (CollectionStorageService.IsInitialized)
                {
                    OnCollectionInitialized();
                }
            }
        }

        private async void OnCollectionInitialized()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;

            if (CollectionStorageService.Collection.Count == 0)
            {
                if (await XmlService.HasOldCollectionFormatAsync())
                {
                    DispatcherService.InvokeOnUIThread(() =>
                        MessageDialogService.Show(Dialogs.IMPORTING_COLLECTION, new Dictionary<string, object>
						{
							{"MessageUid", "ImportCollection_FromOldFormat"}
						}));

                    LoggerService.Log("==> Importing collection from old format");
                    await XmlService.InitializeCollectionFromOldFormatAsync();
                    LoggerService.Log("# Import finished");
                }
                else
                {
                    DispatcherService.InvokeOnUIThread(() =>
                        MessageDialogService.Show(Dialogs.IMPORTING_COLLECTION, new Dictionary<string, object>
						{
							{"MessageUid", "ImportCollection_FromZip"}
						}));

                    LoggerService.Log("==> Importing collection from zip file");
                    await XmlService.InitializeCollectionFromZipStreamAsync(ResourceService.OpenZip("indiagrams.zip"));
                }
                LoggerService.Log("# Import finished");
                MessageDialogService.DismissCurrentDialog();
            }
        }
        protected override void IndiagramSelectedAction(Indiagram indiagram)
        {

            if (indiagram.HasCustomSound)
                MediaService.PlaySound(indiagram.SoundPath);
            else
                TtsService.ReadText(indiagram.Text);
            if (indiagram.IsCategory)
            {
                PushCategory((Category) indiagram);
            }
            else
            {
                ToPlayedList.Add(indiagram);
                RaisePropertyChanged("ToPlayedList");
                if(SettingsService.IsBackHomeAfterSelectionEnabled)
                    while (PopCategory()){}
            }
        }
        protected override IEnumerable<Indiagram> FilterCollection(IEnumerable<Indiagram> input)
        {

            return input.Where(indiagram => ToPlayedList.All(india => !Indiagram.AreSameIndiagram(indiagram, india)));
        }        
    }
}
