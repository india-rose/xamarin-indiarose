using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
    public class ImageChoiceViewModel : AbstractViewModel
    {

        public ImageChoiceViewModel(IContainer container) : base(container)
        {
            //Common ??
            // path : "image/*"
/*
        public void StartRead(string path)
        {
            Initialize(path);
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
                StopRead(data.Data);
            }
        }

        public string StopRead(Uri data)
        {
            //set axml
            return data.Path;
        }
        /*
         * 
         * 
         * TODO Recuperer le chemin de l'image a la fin en utilisant StopRead();
         * 
         * 
         * 
         */
/*
        public void StartWrite(string path)
        {
            throw new System.NotImplementedException();
        }

        public string StopWrite()
        {
            throw new System.NotImplementedException();
        }
        }
    */
        }
    }
}