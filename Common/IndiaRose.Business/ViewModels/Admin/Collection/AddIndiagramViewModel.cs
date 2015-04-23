using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class AddIndiagramViewModel : AbstractSettingsViewModel
    {
        #region MessageDialog

        public IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }
        #endregion

        #region Command
        public ICommand ImageChoiceCommand { get; private set; }
        public ICommand SoundChoiceCommand { get; private set; }
        //Bind with activity
        public ICommand RootCommand { get; private set; }

        #endregion

        [NavigationParameter]
        public Indiagram CurrentIndiagram { get; set; }

        public AddIndiagramViewModel()
        {
            ImageChoiceCommand = new DelegateCommand(ImageChoiceAction);
            SoundChoiceCommand = new DelegateCommand(SoundChoiceAction);
        }

        protected override void SaveAction()
        {
            BackAction();
        }

        protected void ImageChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_IMAGECHOICE);
        }

        protected void SoundChoiceAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SOUNDCHOICE);
        }
    }
}
