using System.Collections.Generic;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using IndiaRose.Storage;
using IndiaRose.Storage.Sqlite;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
	public class SelectCategoryViewModel : AbstractBrowserViewModel
	{

		private IndiagramContainer _currentIndiagram;
		private IndiagramContainer _addIndiagramContainer;

		[NavigationParameter]
		public IndiagramContainer AddIndiagramContainer
		{
			get { return _addIndiagramContainer; }
			set { SetProperty(ref _addIndiagramContainer, value); }
		}
		public IndiagramContainer CurrentIndiagram
		{
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}

	/*	protected virtual IEnumerable<Indiagram> FilterCollection(List<Indiagram> input)
		{
			List<Indiagram> send = new List<Indiagram>();
			foreach (Indiagram indiagram in input)
			{
				if (LazyResolver<ICollectionStorageService>.Service.GetChildren(indiagram).Count != 0)
					send.Add(indiagram);
				else
					;
			}
			return send;
		}*/

		protected override void IndiagramSelectedAction(Indiagram indiagram)
		{
			base.IndiagramSelectedAction(indiagram);
			MessageDialogService.Show(Business.Dialogs.ADMIN_COLLECTION_SELECT, new Dictionary<string, object>()
             {
                 {"CurrentIndiagram",new IndiagramContainer(indiagram)},{"AddIndiagram",AddIndiagramContainer}
             });
		}
	}
}
