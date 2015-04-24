﻿using System.Collections.ObjectModel;
using System.Reflection.Context;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
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
        #endregion

        #region Command
        public ICommand ImageChoiceCommand { get; private set; }
        public ICommand SoundChoiceCommand { get; private set; }
        public ICommand RootCommand { get; private set; }

        #endregion

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

        public AddIndiagramViewModel()
        {
            ImageChoiceCommand = new DelegateCommand(ImageChoiceAction);
            SoundChoiceCommand = new DelegateCommand(SoundChoiceAction);
            RootCommand = new DelegateCommand(RootAction);
            CurrentIndiagram = InitialIndiagram == null ? new Indiagram() : new Indiagram(CurrentIndiagram);
        }

        protected override void SaveAction()
        {
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
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_IMAGECHOICE);
        }

        protected void SoundChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SOUNDCHOICE);
        }

        protected void RootAction()
        {
            CurrentIndiagram.Parent = null;
        }
    }
}
