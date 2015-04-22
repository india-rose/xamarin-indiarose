using System;
using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ImageChoiceViewModel : AbstractViewModel
    {
        public ICommand CameraCommand { get; private set; }
            
	    private IMediaService _mediaService;
        public IMediaService MediaService
        {
            get { return _mediaService ?? (_mediaService = Container.Resolve<IMediaService>()); }
        }
        public ImageChoiceViewModel(IContainer container) : base(container)
        {
            CameraCommand = new DelegateCommand(CameraAction);
        }

        public void CameraAction()
        {
            MediaService.Camera();
            
        }
    }
}