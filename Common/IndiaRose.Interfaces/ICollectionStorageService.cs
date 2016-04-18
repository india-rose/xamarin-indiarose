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
        
        /// <summary>
        /// La totalité des Indiagrams de l'application sous la forme d'une ObservableCollection
        /// </summary>
		ObservableCollection<Indiagram> Collection { get; }

        /// <summary>
        /// Attribut à true si la collection est chargé
        /// Doit lever l'événement Initialized lorsque l'événement est passé à true
        /// </summary>
		bool IsInitialized { get; }

        /// <summary>
        /// Événement indiquant le fin du chargement de la collection d'Indiagram
        /// </summary>
		event EventHandler Initialized;

        /// <summary>
        /// Fonction asynchrone chargeant la collection d'Indiagram
        /// </summary>
        /// <returns>La tâche asynchrone du chargement</returns>
		Task InitializeAsync();

        /// <summary>
        /// Rajoute un Indiagram dans la collection
        /// L'Indiagram retourné est légèrement différent (l'id a été fixé par exemple)
        /// </summary>
        /// <param name="indiagram">L'Indiagram à rajouter dans la collection</param>
        /// <returns>L'Indiagram tel qu'ajouté à la collection</returns>
		Indiagram Save(Indiagram indiagram);

        /// <summary>
        /// Supprime un Indiagram de la collection
        /// Doit supprimer le lien depuis le parent de l'Indiagram, ses fils, et lui et ses fils de la BD
        /// </summary>
        /// <param name="indiagram"></param>
		void Delete(Indiagram indiagram);
	}
}
