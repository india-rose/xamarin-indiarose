using Storm.Mvvm;

namespace IndiaRose.Data.Model
{
    /// <summary>
    /// Classe décrivant les préfèrences utilisateurs
    /// Sert pour le chargement et la sauvegarde
    /// </summary>
    public class SettingsModel : NotifierBase
    {
        public string TopBackgroundColor { get; set; }

        public string BottomBackgroundColor { get; set; }

        public string ReinforcerColor { get; set; }

        public string TextColor { get; set; }

        public int SelectionAreaHeight { get; set; }

        public int IndiagramDisplaySize { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; }

        public bool IsReinforcerEnabled { get; set; }

        public bool IsDragAndDropEnabled { get; set; }

        public bool IsCategoryNameReadingEnabled { get; set; }

        public bool IsBackHomeAfterSelectionEnabled { get; set; }

        public bool IsMultipleIndiagramSelectionEnabled { get; set; }

        public float TimeOfSilenceBetweenWords { get; set; }

        public bool IsBackButtonEnabled { get; set; }
    }
}
