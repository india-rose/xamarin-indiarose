using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace IndiaRose.Core.Admins.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		public ReactiveCommand<Unit, bool> SaveCommand { get; private set; }

		public SettingsViewModel()
		{
			SaveCommand = ReactiveCommand.CreateFromTask(async _ =>
			{
				await Task.Delay(1000);
				return new Random((int)DateTime.Now.Millisecond).Next(0, 10) < 5;
			});
		}
	}
}
