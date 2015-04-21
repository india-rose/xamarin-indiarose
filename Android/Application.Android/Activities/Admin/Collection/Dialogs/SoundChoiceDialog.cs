using Android.Media;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
    public partial class SoundChoiceDialog : AlertDialogFragmentBase
    {

        private MediaRecorder _recorder;
        private string _url;

        public SoundChoiceDialog()
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
            Title = trad.GetString("CS_Title", "Text");
            Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Ok", "Text"));
            Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
        }
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_SoundChoiceDialog, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminCollectionDialogsSoundChoiceDialog;
        }

        public void StartWrite(string url)
        {
            _url = url;
            _recorder = new MediaRecorder();
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            _recorder.SetOutputFile(_url);
            _recorder.Prepare();
            _recorder.Start();
        }

        public string StopWrite()
        {
            _recorder.Stop();
            _recorder.Reset();

            return _url;
        }
    }
}