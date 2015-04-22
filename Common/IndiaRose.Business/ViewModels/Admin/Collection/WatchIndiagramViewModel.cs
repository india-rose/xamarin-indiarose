using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class WatchIndiagramViewModel : AbstractSettingsViewModel
    {
        public ICommand EditCommand { get; private set; }
        [NavigationParameter]
        public Indiagram CurrentIndiagram { get; private set; }

         public WatchIndiagramViewModel(IContainer container)
            : base(container)
        {
             EditCommand=new DelegateCommand(EditAction);
             CurrentIndiagram = new Indiagram("test", "5317034a965b6suer.png");
        }
         private void EditAction()
         {
             NavigationService.Navigate(Views.ADMIN_COLLECTION_ADD,new Dictionary<string, object>()
             {
                 {"CurrentIndiagram",CurrentIndiagram}
             });
         }
    }
}
