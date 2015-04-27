using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class RecordSoundViewModel : AbstractViewModel
    {
        #region Service
        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }
        public IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }
        #endregion
        #region Command
        public ICommand CloseCommand { get; private set; }
        public ICommand RecordCommand { get; private set; }
        #endregion
        public RecordSoundViewModel()
        {
            CloseCommand=new DelegateCommand(CloseAction);
            RecordCommand=new DelegateCommand(RecordAction);
        }
        #region Action
        protected void CloseAction()
        {
            MessageDialogService.DismissCurrentDialog();
        }

        protected void RecordAction()
        {
            MessageDialogService.DismissCurrentDialog();

        }
        #endregion
    }
}
