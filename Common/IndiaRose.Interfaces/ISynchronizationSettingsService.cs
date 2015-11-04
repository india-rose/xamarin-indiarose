using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
	public interface ISynchronizationSettingsService
	{
		string UserLogin { get; set; }

		string UserPasswd { get; set; }

		bool UserRememberMe { get; set; }

		string DeviceName { get; set; }


		long CollectionVersion { get; set; }

		long CollectionLastId { get; set; }

		string CollectionLastContent { get; set; }


		long SettingsVersion { get; set; }

		string SettingsLastContent { get; set; }


		Task SaveAsync();

		Task LoadAsync();

		void Reset();
	}
}
