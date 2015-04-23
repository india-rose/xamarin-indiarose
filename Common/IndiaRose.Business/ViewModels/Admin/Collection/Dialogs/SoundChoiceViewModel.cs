using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class SoundChoiceViewModel : AbstractViewModel
    {

        #region Service & Command

        private IMessageDialogService _messageDialogService;
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
        public SoundChoiceViewModel()
        {
            RecordSoundCommand = new DelegateCommand(RecordSoundAction);
            GalleryCommand=new DelegateCommand(GalleryAction);
        }

        private void RecordSoundAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_RECORDSOUND);
        }

        private void GalleryAction()
        {
            MediaService.GetSoundFromGalleryAsync();
        }
    }
}
