using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.Test;
using IndiaRose.Framework;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using Uri = Android.Net.Uri;


namespace IndiaRose.Services.Android
{
    public class ResourceService : AbstractAndroidService, IResourceService
    {
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
                catch (Exception)
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

		public Task<Stream> OpenZip(string zipFileName)
		{
			return AsyncHelper.CreateAsyncFromCallback<Stream>(callbackResult =>
			{
				using (Stream input = CurrentActivity.Assets.Open(zipFileName))
				{
					MemoryStream inputStream = new MemoryStream();
					input.CopyTo(inputStream);
					inputStream.Position = 0;
					callbackResult(inputStream);
				}
			});
		}

        public void Copy(string src,string dest)
        {
            FileStream output = File.OpenWrite(dest);
            Stream input = CurrentActivity.Assets.Open(src);
            input.CopyTo(output);
            input.Close();
            output.Flush();
            output.Close();
        }
    }
}