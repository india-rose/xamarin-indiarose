using System.Collections.Generic;
using IndiaRose.Data.Model;

namespace IndiaRose.Storage
{
	public interface ICollectionStorageService
	{
		void Create(Indiagram indiagram);

		void Update(Indiagram indiagram);

		void Delete(Indiagram indiagram);

		List<Indiagram> GetTopLevel();

		List<Indiagram> GetChildren(Indiagram parent);

		List<Indiagram> GetFullCollection();

        //TODO delete after test
	    void DropTable();
	}
}
