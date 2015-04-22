using System.Windows.Input;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class SoundChoiceViewModel : AbstractViewModel
    {

        #region MessageDialog

        private IMessageDialogService _messageDialogService;
        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }
        #endregion
        public ICommand RecordSoundCommand { get; private set; }
        public SoundChoiceViewModel()
        {
            RecordSoundCommand = new DelegateCommand(RecordSoundAction);
        }

        public void RecordSoundAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_RECORDSOUND);
        }
    }
}
