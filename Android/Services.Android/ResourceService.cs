using System;
using System.IO;
using Android.Content;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using Uri = Android.Net.Uri;

namespace IndiaRose.Services.Android
{
    public class ResourceService : AbstractAndroidService, IResourceService
    {
		public ResourceService(IContainer container)
			: base(container)
	    {
	    }

	    public void ShowPdfFile(string pdfFileName)
        {
            string path = Path.Combine(CurrentActivity.GetExternalFilesDir(null).AbsolutePath, pdfFileName);

            if (!File.Exists(path))
            {
                try
                {
                    FileStream output = File.OpenWrite(path);
					Stream input = CurrentActivity.Assets.Open(pdfFileName);
                    input.CopyTo(output);
                    input.Close();
                    output.Flush();
                    output.Close();
                }
                catch (Exception e)
                {
                    LoggerService.Log(string.Format("ResourceService.ShowPdfFile() : Cannot create external file {0}", path), MessageSeverity.Error);
                }
            }
            try
            {
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(Uri.Parse("file://" + path), "application/pdf");
				CurrentActivity.StartActivity(intent);
            }
            catch (Exception)
            {
                try
                {
					CurrentActivity.StartActivity(new Intent(Intent.ActionView, Uri.Parse("market://search?q=pdf&c=apps")));
                }
                catch (ActivityNotFoundException)
                {
					CurrentActivity.StartActivity(new Intent(Intent.ActionView, Uri.Parse("https://play.google.com/store/search?q=pdf&c=apps")));
                }
            }

        }
    }
}