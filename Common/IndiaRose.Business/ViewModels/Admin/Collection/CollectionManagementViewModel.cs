using System.Windows.Input;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Business.ViewModels.Admin.Collection
{
    public class CollectionManagementViewModel : AbstractViewModel
    {
        public CollectionManagementViewModel(IContainer container)
            : base(container)
        {
        }
    }
}
