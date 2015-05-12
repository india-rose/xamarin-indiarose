using System;
using System.Threading.Tasks;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
	public class MediaService : IMediaService
	{
		public Task<string> GetPictureFromCameraAsync()
		{
			throw new NotImplementedException();
		}

		public Task<string> GetPictureFromGalleryAsync()
		{
			throw new NotImplementedException();
		}

		public Task<string> GetSoundFromGalleryAsync()
		{
			throw new NotImplementedException();
		}

		public void RecordSound()
		{
			throw new NotImplementedException();
		}

		public string StopRecord()
		{
			throw new NotImplementedException();
		}

		public void PlaySound(string url)
		{
			throw new NotImplementedException();
		}
	}
}