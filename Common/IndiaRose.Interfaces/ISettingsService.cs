using System.Threading.Tasks;

namespace IndiaRose.Interfaces
{
    public interface ISettingsService
    {
		string TopBackgroundColor { get; set; }

		string BottomBackgroundColor { get; set; }

		string ReinforcerColor { get; set; }

        string TextColor { get; set; }

		int SelectionAreaHeight { get; set; }

		int IndiagramDisplaySize { get; set; }

		string FontName { get; set; }

		int FontSize { get; set; }

		bool IsReinforcerEnabled { get; set; }

		bool IsDragAndDropEnabled { get; set; }

		bool IsCategoryNameReadingEnabled { get; set; }

		bool IsBackHomeAfterSelectionEnabled { get; set; }

		float TimeOfSilenceBetweenWords { get; set; }

		Task SaveAsync();

		Task LoadAsync();

		void Reset();
    }
}
