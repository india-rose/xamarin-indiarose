using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace IndiaRose.Core.Admins.ViewModels
{
	public abstract class BaseViewModel : ReactiveObject, ISupportsActivation
	{
		protected BaseViewModel()
		{
			Activator = new ViewModelActivator();
		}

		public ViewModelActivator Activator { get; }
	}
}
