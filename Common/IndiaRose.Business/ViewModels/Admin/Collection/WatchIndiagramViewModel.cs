using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.Data.Model;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class WatchIndiagramViewModel : AbstractViewModel
    {
        public ICommand EditCommand { get; private set; }
        [NavigationParameter]
        public Indiagram CurrentIndiagram { get; private set; }

         public WatchIndiagramViewModel(IContainer container)
            : base(container)
        {
             EditCommand=new DelegateCommand(EditAction);
             CurrentIndiagram=new Indiagram("test",null);
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
