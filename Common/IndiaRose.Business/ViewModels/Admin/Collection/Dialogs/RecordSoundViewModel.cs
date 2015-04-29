using System.Windows.Input;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
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
        public ICommand StopCommand { get; private set; }
        #endregion
        #region Properties
        private IndiagramContainer _indiagramContainer;
        private bool _isRecording;

        [NavigationParameter]
        public IndiagramContainer Indiagram
        {
            get { return _indiagramContainer;}
            set { SetProperty(ref _indiagramContainer, value); }
        }

        public bool IsRecording
        {
            get { return _isRecording;}
            set { SetProperty(ref _isRecording, value); }
        }
        #endregion
        public RecordSoundViewModel()
        {
            CloseCommand=new DelegateCommand(CloseAction);
            RecordCommand=new DelegateCommand(RecordAction);
            StopCommand=new DelegateCommand(StopAction);
        }
        #region Action
        protected void CloseAction()
        {
            MessageDialogService.DismissCurrentDialog();
        }
        protected void RecordAction()
        {
            MediaService.RecordSound();
            IsRecording = true;
        }
        protected void StopAction()
        {
            Indiagram.Indiagram.SoundPath=MediaService.StopRecord();
            IsRecording = false;
            MessageDialogService.DismissCurrentDialog();
        }
        #endregion
    }
}
