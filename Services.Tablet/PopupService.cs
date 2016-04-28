using IndiaRose.Interfaces;

namespace Services.Tablet
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
