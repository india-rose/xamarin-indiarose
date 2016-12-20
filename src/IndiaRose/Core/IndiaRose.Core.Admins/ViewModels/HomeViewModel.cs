using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IndiaRose.Core.Admins.Interfaces;
using ReactiveUI;
using Splat;

namespace IndiaRose.Core.Admins.ViewModels
{
	public class HomeViewModel : ReactiveObject
	{
		public ICommand OpenAppSettingsCommand { get; private set; }

		public HomeViewModel()
		{
			OpenAppSettingsCommand = ReactiveCommand.Create(() => Locator.Current.GetService<INavigationService>().ShowAppSettings());
		}
	}
}
