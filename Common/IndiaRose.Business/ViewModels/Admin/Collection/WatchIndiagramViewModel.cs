using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class WatchIndiagramViewModel : AbstractViewModel
    {
         public WatchIndiagramViewModel(IContainer container)
            : base(container)
        {
        }
    }
}
