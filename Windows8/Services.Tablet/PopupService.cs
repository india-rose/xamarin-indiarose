using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
	public class PopupService : IPopupService
	{
		public void DisplayPopup(string message)
		{
			var msg = new Windows.UI.Popups.MessageDialog(message);
			msg.ShowAsync();
		}
		 
	}
}
