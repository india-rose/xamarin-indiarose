using System;
using System.Windows.Input;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ImageChoiceViewModel : AbstractViewModel
    {
        #region Command & Service
        public ICommand CameraCommand { get; private set; }
        public ICommand GalleryCommand { get; private set; }
            
        public IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }
        #endregion

        [NavigationParameter]
        public Indiagram Indiagram { get; set; }

        public ImageChoiceViewModel()
        {
            CameraCommand = new DelegateCommand(CameraAction);
            GalleryCommand = new DelegateCommand(GalleryAction);
        }

        public async void CameraAction()
        {
            Indiagram.ImagePath = await MediaService.GetPictureFromCameraAsync();
        }

        public async void GalleryAction()
        {
            Indiagram.ImagePath = await MediaService.GetPictureFromGalleryAsync();
        }
    }
}