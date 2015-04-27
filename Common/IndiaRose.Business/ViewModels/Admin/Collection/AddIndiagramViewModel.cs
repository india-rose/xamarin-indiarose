using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using IndiaRose.Storage;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class AddIndiagramViewModel : AbstractSettingsViewModel
    {
        #region Service

        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }

        public ICollectionStorageService CollectionStorageService
        {
            get { return LazyResolver<ICollectionStorageService>.Service; }
        }

        public IPopupService PopupService
        {
            get { return LazyResolver<IPopupService>.Service; }
        }

        public ILocalizationService LocalizationService
        {
            get { return LazyResolver<ILocalizationService>.Service; }
        }
        #endregion

        #region Command
        public ICommand ImageChoiceCommand { get; private set; }
        public ICommand SoundChoiceCommand { get; private set; }
        public ICommand RootCommand { get; private set; }
        public ICommand ResetSoundCommand { get; private set; }
        public ICommand ListenCommand { get; private set; }
        public ICommand ChooseCategoryCommand { get; private set; }
        public ICommand ActivateCommand { get; private set; }
        public ICommand DesactivateCommand { get; private set; }

        #endregion

        #region Properties
        private string _bro1;

        public string Bro1
        {
            get { return _bro1; }
            set { SetProperty(ref _bro1, value); }
        }
        public ObservableCollection<string> Brothers { get; private set; }


        [NavigationParameter]
        protected Indiagram InitialIndiagram { get; set; }

        public Indiagram CurrentIndiagram { get; set; }
        #endregion

        public AddIndiagramViewModel()
        {
            ImageChoiceCommand = new DelegateCommand(ImageChoiceAction);
            SoundChoiceCommand = new DelegateCommand(SoundChoiceAction);
            RootCommand = new DelegateCommand(RootAction);
            ResetSoundCommand=new DelegateCommand(ResetSoundAction);
            ListenCommand=new DelegateCommand(ListenAction);
            ChooseCategoryCommand=new DelegateCommand(ChooseCategoryAction);
            ActivateCommand = new DelegateCommand(ActivateAction);
            DesactivateCommand = new DelegateCommand(DesactivateAction);
            CurrentIndiagram = InitialIndiagram == null ? new Indiagram() : new Indiagram(CurrentIndiagram);
        }

        #region Action

        protected void ActivateAction()
        {
            CurrentIndiagram.IsEnabled = true;
        }
        protected void DesactivateAction()
        {
            CurrentIndiagram.IsEnabled = false;
        }
        protected void ChooseCategoryAction()
        {
            //TODO a faire avec le frag
        }
        protected override void SaveAction()
        {
            if (CurrentIndiagram.Text == null)
            {
                PopupService.AfficherPopup(LocalizationService.GetString("AIP_NoName","Text"));
                return;
            }
            if (InitialIndiagram == null)
            {
                InitialIndiagram = CurrentIndiagram;
                CollectionStorageService.Create(InitialIndiagram);
            }
            else
            {
                InitialIndiagram.Edit(CurrentIndiagram);
                CollectionStorageService.Update(InitialIndiagram);
            }

            BackAction();
        }
        protected void ImageChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_IMAGECHOICE,new Dictionary<string, object>
            {
                {"Indiagram",CurrentIndiagram}
            });
        }
        protected void SoundChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SOUNDCHOICE, new Dictionary<string, object>
            {
                {"Indiagram",CurrentIndiagram}
            });
        }
        protected void RootAction()
        {
            CurrentIndiagram.Parent = null;
        }
        protected void ResetSoundAction()
        {
            CurrentIndiagram.SoundPath = null;
        }
        protected void ListenAction()
        {
            if (CurrentIndiagram.Text == null)
            {
                PopupService.AfficherPopup(LocalizationService.GetString("AIP_NoSoundError", "Text"));
            }
            
        }

        #endregion
    }
}
