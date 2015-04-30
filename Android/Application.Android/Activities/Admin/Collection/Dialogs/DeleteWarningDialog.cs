using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
    [BindingElement(Path = "DeleteCommand", TargetPath = "PositiveButtonEvent")]
    public partial class DeleteWarningDialog : AlertDialogFragmentBase
    {
        public DeleteWarningDialog()
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
            Title = trad.GetString("DelWarning_Title", "Text");
            Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Ok", "Text"));
            Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
        }
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_DeleteWarningDialog, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminCollectionDialogDeleteWarningDialog;
        }
    }
}