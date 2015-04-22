using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ImageChoiceViewModel : AbstractViewModel
    {
        public ICommand GalleryCommand;
            
        public IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }

        public ImageChoiceViewModel()
        {
        }
    }
}