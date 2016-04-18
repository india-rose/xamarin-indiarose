using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    /// <summary>
    /// VueModèle abstrait pour les pages modifiant la collection
    /// </summary>
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
