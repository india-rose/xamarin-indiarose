using System;
using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

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

        public ImageChoiceViewModel()
        {
            CameraCommand = new DelegateCommand(CameraAction);
            GalleryCommand = new DelegateCommand(GalleryAction);
        }

        public async void CameraAction()
        {
            string imagePath = await MediaService.GetPictureFromCameraAsync();
            //TODO : faire quelque chose avec imagePath.
        }

        public async void GalleryAction()
        {
            string imagePath = await MediaService.GetPictureFromGalleryAsync();
            //TODO : IDEM
        }
    }
}