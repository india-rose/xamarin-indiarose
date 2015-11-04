namespace IndiaRose.Data.Model
{
	public class SynchronizationSettingsModel
	{
		public string UserLogin { get; set; }

		public string UserPasswd { get; set; }
		

		public long CollectionVersion { get; set; }
		
		public long CollectionLastId { get; set; }
		
		public string CollectionLastContent { get; set; }
		

		public long SettingsVersion { get; set; }

		public string SettingsLastContent { get; set; }
	}
}
