using Android.Widget;
using IndiaRose.Interfaces;

namespace IndiaRose.Services.Android
{
    public class PopupService : AbstractAndroidService,IPopupService
    {
        public void AfficherPopup(string message)
        {
            Toast.MakeText(CurrentActivity, message, ToastLength.Long).Show();
        }
    }
}