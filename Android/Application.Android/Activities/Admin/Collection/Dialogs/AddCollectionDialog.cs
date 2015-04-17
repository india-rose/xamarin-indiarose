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
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
    [Activity(Label = "AddCollectionDialog")]
    public partial class AddCollectionDialog : AlertDialogFragmentBase
    {
        public AddCollectionDialog()
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
            Title = trad.GetString("whichActionQuestion", "Text");

            Buttons.Add(DialogsButton.Positive, trad.GetString("goIntoText", "Text"));
            Buttons.Add(DialogsButton.Negative, trad.GetString("seeText", "Text"));
        }

        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_AddCollectionDialog, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminCollectionDialogsAddCollectionDialog;
        }
    }
}