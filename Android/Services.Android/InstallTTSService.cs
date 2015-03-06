using Android.Content;
using Android.Net;
using IndiaRose.Interfaces;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;

namespace IndiaRose.Services.Android
{
	// Disable inconsistent naming for this only
	// ReSharper disable once InconsistentNaming
    public class InstallTTSService : AbstractServiceWithActivity, IInstallVoiceSynthesisService
    {
	    public InstallTTSService(IActivityService activityService) : base(activityService)
	    {
	    }

	    public void InstallVoiceSynthesisEngine()
        {
            Intent intent = GoToMarket("https://play.google.com/store/apps/details?id=com.ivona.tts&hl=fr");
            CurrentActivity.StartActivity(intent);
        }

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