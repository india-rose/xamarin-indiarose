namespace IndiaRose.Core.Models
{
	/// <summary>
	/// User settings 
	/// </summary>
	public class Settings
	{
		//colors
		public string TopBackgroundColor { get; set; }

		public string BottomBackgroundColor { get; set; }

		public string ReinforcerColor { get; set; }

		public string TextColor { get; set; }

		//size
		public int SelectionAreaHeight { get; set; }

		public int IndiagramDisplaySize { get; set; }

		//font
		public string FontName { get; set; }

		public int FontSize { get; set; }

		//options
		public bool IsReinforcerEnabled { get; set; }

		public bool IsDragAndDropEnabled { get; set; }

		public bool IsCategoryNameReadingEnabled { get; set; }

		public bool IsBackHomeAfterSelectionEnabled { get; set; }

		public bool IsMultipleIndiagramSelectionEnabled { get; set; }

		public bool IsBackButtonEnabled { get; set; }

		//word reading options
		public float TimeOfSilenceBetweenWords { get; set; }
	}
}
