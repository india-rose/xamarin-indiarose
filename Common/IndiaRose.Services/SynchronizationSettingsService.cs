using System;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Newtonsoft.Json;
using Storm.Mvvm.Services;

namespace IndiaRose.Services
{
	public class SynchronizationSettingsService : AbstractFileStorageService, ISynchronizationSettingsService
	{
		public string UserLogin { get; set; }
		public string UserPasswd { get; set; }

		public long CollectionVersion { get; set; }
		public long CollectionLastId { get; set; }
		public string CollectionLastContent { get; set; }

		public long SettingsVersion { get; set; }
		public string SettingsLastContent { get; set; }

		public SynchronizationSettingsService()
		{
			FolderPath = StorageService.SynchronizationFolderPath;
			FileName = StorageService.SynchronizationFileName;
		}

		public async Task LoadAsync()
		{
			if (!await ExistsOnDiskAsync())
			{
				Reset();
				return;
			}

			SynchronizationSettingsModel model;
			try
			{
				model = JsonConvert.DeserializeObject<SynchronizationSettingsModel>(await LoadFromDiskAsync());
			}
			catch (Exception e)
			{
				LoggerService.Log("IndiaRose.Services.SynchronizationSettingsService.LoadAsync() : cannot deserialize synchronization settings file content " + e, MessageSeverity.Critical);

				Reset();
				return;
			}

			UserLogin = model.UserLogin;
			UserPasswd = model.UserPasswd;
			CollectionVersion = model.CollectionVersion;
			CollectionLastId = model.CollectionLastId;
			CollectionLastContent = model.CollectionLastContent;
			SettingsVersion = model.SettingsVersion;
			SettingsLastContent = model.SettingsLastContent;
		}

		public async Task SaveAsync()
		{
			SynchronizationSettingsModel model = new SynchronizationSettingsModel
			{
				UserLogin = UserLogin,
				UserPasswd = UserPasswd,
				CollectionVersion = CollectionVersion,
				CollectionLastId = CollectionLastId,
				CollectionLastContent = CollectionLastContent,
				SettingsVersion = SettingsVersion,
				SettingsLastContent = SettingsLastContent,
			};
			await SaveToDiskAsync(JsonConvert.SerializeObject(model, Formatting.Indented));
		}

		public void Reset()
		{
			UserLogin = null;
			UserPasswd = null;

			CollectionVersion = -1;
			CollectionLastId = -1;
			CollectionLastContent = null;

			SettingsVersion = -1;
			SettingsLastContent = null;
		}
	}
}
