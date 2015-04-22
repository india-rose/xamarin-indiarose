using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Views;
using Android.Widget;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
    public partial class ImageChoiceDialog : AlertDialogFragmentBase, IMedia
    {
        public ImageChoiceDialog()
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
            Title = trad.GetString("CI_Title", "Text");
            Buttons.Add(DialogsButton.Positive, trad.GetString("Button_Ok", "Text"));
            Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
        }
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_ImageChoiceDialog, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminCollectionDialogsImageChoiceDialog;
        }
        // path : "image/*"
        public void StartWrite(string path)
        {
            throw new System.NotImplementedException();
        }

        public string StopWrite()
        {
            throw new System.NotImplementedException();
        }

        public string StopRead(System.Uri data)
        {
            throw new System.NotImplementedException();
        }

        protected void Initialize(string path)
        {
            var imageIntent = new Intent();
                //chemin fonctionel sur nexus
                imageIntent.SetType(path);
                imageIntent.SetAction(Intent.ActionGetContent);
                //SartActivityForResult attend resultat
                StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }


        //result de SartActivityForResult a tester une fois binde
        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            //a voir
            base.OnActivityResult(requestCode, resultCode, data);

            //verif result activity
            if (resultCode == Result.Ok)
            {
                StopRead(data.Data.ToString());
            }
        }

        public string StopRead(string data)
        {
            //set axml
            return data;
        }
        /*
         * 
         * 
         * TODO Recuperer le chemin de l'image a la fin en utilisant StopRead();
         * 
         * 
         * 
         */
    }
}