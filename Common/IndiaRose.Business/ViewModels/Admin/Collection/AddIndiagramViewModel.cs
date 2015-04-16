using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Business.ViewModels.Admin.Settings;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class AddIndiagramViewModel : AbstractSettingsViewModel
    {
        public AddIndiagramViewModel(IContainer container) : base(container)
        {
            
        }

        protected override void SaveAction()
        {
            BackAction();
        }
    }
}
