using System;
using System.Threading;
using System.Threading.Tasks;
using IndiaRose.Core.Interfaces;
using IndiaRose.Core.Models;

namespace IndiaRose.Core.Services
{
	public class SettingsService : ISettingsService
	{
		private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);
		private Settings _settings;

		public async Task<Settings> Load(CancellationToken ct)
		{
			if (_settings != null)
			{
				return _settings;
			}

			await _mutex.WaitAsync(ct);
			try
			{
				_settings = new Settings
				{
					TopBackgroundColor = "#FF00FF",
					BottomBackgroundColor = "#FF0000",
					FontSize = 14,
					IndiagramSizePercentage = 80
				};
				return _settings;
			}
			catch (Exception ex)
			{
				ServiceLocator.LoggerService.Exception(ex);
				return null;
			}
			finally
			{
				_mutex.Release();
			}
		}

		public Task<Settings> Load()
		{
			return Load(CancellationToken.None);
		}

		public Task<bool> Save(Settings settings)
		{
			return Save(settings, CancellationToken.None);
		}

		public Task<bool> Save(Settings settings, CancellationToken ct)
		{
			_settings = settings;
			return Task.FromResult(true);
		}
	}
}
