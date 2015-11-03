using IndiaRose.WebAPI.Sdk.Services;
using ModernHttpClient;

namespace IndiaRose.Services.Android
{
	public class WebApiRequestService : RequestServiceBase
	{
		public WebApiRequestService() : base(new NativeMessageHandler())
		{
			
		}
	}
}