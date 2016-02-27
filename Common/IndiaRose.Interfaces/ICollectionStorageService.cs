using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    /// <summary>
    /// Fournit des méthodes pour la gestion de la collection d'Indiagram
    /// </summary>
	public interface ICollectionStorageService
	{
        //TODO
		ObservableCollection<Indiagram> Collection { get; }

		bool IsInitialized { get; }

		event EventHandler Initialized;

		Task InitializeAsync();

		Indiagram Save(Indiagram indiagram);

		void Delete(Indiagram indiagram);
	}
}
