using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    class CollectionManagementActivityViewModel : AbstractViewModel
    {
        public CollectionManagementActivityViewModel(IContainer container)
            : base(container)
        {
        }
    }
}
