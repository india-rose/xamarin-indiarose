using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.Dialogs;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
    public partial class RecordSoundDialog : AlertDialogFragmentBase
    {
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Views_Admin_Collection_Dialogs_RecordSoundDialog, container, false);
        }

        protected override Storm.Mvvm.ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminCollectionDialogsRecordSoundDialog;
        }
    }
}