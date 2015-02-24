using Storm.Mvvm;

namespace IndiaRose.Data.Model
{
	public class SettingsModel : NotifierBase
	{
		public uint TopBackgroundColor { get; set; }
		
		public uint BottomBackgroundColor { get; set; }
		
		public uint ReinforcerColor { get; set; }
		
		public int SelectionAreaHeight { get; set; }
		
		public int IndiagramDisplaySize { get; set; }
		
		public string FontName { get; set; }
		
		public int FontSize { get; set; }
		
		public bool IsReinforcerEnabled { get; set; }
		
		public bool IsDragAndDropEnabled { get; set; }
		
		public bool IsCategoryNameReadingEnabled { get; set; }

		public bool IsBackHomeAfterSelectionEnabled { get; set; }

		public float TimeOfSilenceBetweenWords { get; set; }
	}
}
