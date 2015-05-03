using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	public partial class RecordSoundDialog : AlertDialogFragmentBase
    {
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Views_Admin_Collection_Dialogs_RecordSoundDialog, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminCollectionDialogsRecordSoundViewModel;
        }
    }
}