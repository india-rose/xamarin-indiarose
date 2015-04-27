﻿using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class SoundChoiceViewModel : AbstractViewModel
    {

        #region Service & Command
        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }
        public IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }
        public ICommand RecordSoundCommand { get; private set; }
        public ICommand GalleryCommand { get; private set; }
        #endregion

        [NavigationParameter]
        public Indiagram Indiagram { get; set; }
        public SoundChoiceViewModel()
        {
            RecordSoundCommand = new DelegateCommand(RecordSoundAction);
            GalleryCommand=new DelegateCommand(GalleryAction);
        }

        private void RecordSoundAction()
        {
            MessageDialogService.DismissCurrentDialog();
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_RECORDSOUND);
        }

        private async void GalleryAction()
        {
            Indiagram.SoundPath= await MediaService.GetSoundFromGalleryAsync();
            MessageDialogService.DismissCurrentDialog();
        }
    }
}
