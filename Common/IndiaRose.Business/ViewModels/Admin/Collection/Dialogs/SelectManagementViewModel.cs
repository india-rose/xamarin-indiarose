using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection.Dialogs
{
	public class SelectManagementViewModel : AbstractSettingsViewModel
	{
		private Indiagram _currentIndiagram;

		[NavigationParameter]
		public Indiagram CurrentIndiagram
		{
			get { return _currentIndiagram; }
			set { SetProperty(ref _currentIndiagram, value); }
		}
	}
}
