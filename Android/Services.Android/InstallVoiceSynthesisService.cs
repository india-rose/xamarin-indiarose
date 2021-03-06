using Android.Content;
using Android.Net;
using IndiaRose.Interfaces;

namespace IndiaRose.Services.Android
{
    public class InstallVoiceSynthesisService : AbstractAndroidService, IInstallVoiceSynthesisService
    {
	    public void InstallVoiceSynthesisEngine()
        {
            Intent intent = GoToMarket("https://play.google.com/store/apps/details?id=com.ivona.tts&hl=fr");
            CurrentActivity.StartActivity(intent);
        }

        /// <summary>
        /// Construit un Intent pour lancer le PlayStore
        /// </summary>
        /// <param name="url">L'adresse � laquelle lanc� le PlayStore</param>
        /// <returns>L'Intent lan�ant le PlayStore</returns>
        private Intent GoToMarket(string url)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetData(Uri.Parse(url));
            return intent;
        }

        public void InstallLanguagePack()
        {
            Intent intent = GoToMarket("http://mobile.ivona.com?ap=EMBED&v=1&set_lang=us");
            CurrentActivity.StartActivity(intent);
        }

        public void EnableVoiceSynthesisEngine()
        {
            Intent intent = new Intent();
            intent.SetAction("com.android.settings.TTS_SETTINGS");
            intent.SetFlags(ActivityFlags.NewTask);
            CurrentActivity.StartActivity(intent);
        }
    }
}