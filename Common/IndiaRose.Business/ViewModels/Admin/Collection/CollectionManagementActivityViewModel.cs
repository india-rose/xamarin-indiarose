using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    class CollectionManagementActivityViewModel : AbstractViewModel
    {
        #region Constructor

        public CollectionManagementActivityViewModel(IContainer container) : base(container)
		{
			IsEnabled = SettingsService.IsCategoryNameReadingEnabled;
		}

		#endregion

		#region Commands implementation

		protected override void SaveAction()
		{
			SettingsService.IsCategoryNameReadingEnabled = IsEnabled;
			base.SaveAction();
		}

		#endregion
    }
}
