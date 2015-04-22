using System;
using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ImageChoiceViewModel : AbstractViewModel
    {
        public ICommand CameraCommand { get; private set; }
            
        public IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }

        public ImageChoiceViewModel()
        {
            CameraCommand = new DelegateCommand(CameraAction);
        }

        public void CameraAction()
        {
            MediaService.Camera();
            
        }
    }
}