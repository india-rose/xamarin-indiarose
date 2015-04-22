using System.Collections.Generic;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class ManagementViewModel : AbstractSettingsViewModel
    {
        public List<Indiagram> CurrentIndiagram { get; private set; }

        public ManagementViewModel(IContainer container)
            : base(container)
        {

        }
    }
}
