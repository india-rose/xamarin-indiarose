#region Usings

using Storm.Mvvm;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin
{
	public class ServerSynchronizationViewModel : ViewModelBase
	{
		public ServerSynchronizationViewModel(IContainer container) : base(container)
		{
		}

	    public string test { get { return "plop";  } }
	}
}