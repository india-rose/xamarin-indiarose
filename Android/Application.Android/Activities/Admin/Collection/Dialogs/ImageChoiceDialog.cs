using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
    public partial class ImageChoiceDialog : AlertDialogFragmentBase
    {
        public ImageChoiceDialog()
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
			Title = trad.GetString("ImageChoice_Title", "Text");
            Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Cancel", "Text"));
        }
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_ImageChoiceDialog, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminCollectionDialogsImageChoiceViewModel;
        }
    }
}