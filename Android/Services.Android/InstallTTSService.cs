using Android.Content;
using IndiaRose.Interfaces;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;
using Uri = Android.Net.Uri;
using Intent = Android.Content.Intent;

namespace IndiaRose.Services.Android
{
    public class InstallTTSService : AbstractServiceWithActivity, IInstallTTSService
    {
	    public InstallTTSService(IActivityService activityService) : base(activityService)
	    {
	    }

	    public void InstallIvona()
        {
            Intent intent = goToMarket("https://play.google.com/store/apps/details?id=com.ivona.tts&hl=fr");
            CurrentActivity.StartActivity(intent);
        }

        private Intent goToMarket(string url)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetData(Uri.Parse(url));
            return intent;
        }

        public void InstallPack()
        {
            Intent intent = goToMarket("http://mobile.ivona.com?ap=EMBED&v=1&set_lang=us");
            CurrentActivity.StartActivity(intent);
        }

        public void chooseIvona()
        {
            Intent intent = new Intent();
            intent.SetAction("com.android.settings.TTS_SETTINGS");
            intent.SetFlags(ActivityFlags.NewTask);
            CurrentActivity.StartActivity(intent);
        }
    }
}