using System.Collections.ObjectModel;
using IndiaRose.Data.Model;

namespace IndiaRose.Storage
{
	public interface ICollectionStorageService
	{
		ObservableCollection<Indiagram> Collection { get; } 

		Indiagram Save(Indiagram indiagram);

		void Delete(Indiagram indiagram);
	}
}
