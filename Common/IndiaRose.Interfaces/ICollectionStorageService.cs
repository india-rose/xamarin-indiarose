using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
	public interface ICollectionStorageService
	{
		ObservableCollection<Indiagram> Collection { get; }

		bool IsInitialized { get; }

		event EventHandler Initialized;

		Task InitializeAsync();

		Indiagram Save(Indiagram indiagram);

		void Delete(Indiagram indiagram);
	}
}
