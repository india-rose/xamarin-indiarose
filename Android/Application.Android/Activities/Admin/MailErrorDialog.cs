using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin
{
    public partial class MailErrorDialog : AlertDialogFragmentBase
    {
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Admin_MailErrorDialog, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminMailErrorViewModel;
        }

        public MailErrorDialog()
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
            Title = trad.GetString("Error", "Text");
            Buttons.Add(DialogsButton.Neutral,trad.GetString("Button_Back","Text"));
        }
    }
}