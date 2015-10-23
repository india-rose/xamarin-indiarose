using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class AbstractCollectionViewModel : AbstractViewModel
	{
		public ISettingsService SettingsService
		{
			get { return LazyResolver<ISettingsService>.Service; }
		}

		protected ICollectionStorageService CollectionStorageService
		{
			get { return LazyResolver<ICollectionStorageService>.Service; }
		}
	}
}
