using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Application.Tablet.Views.Dialogs
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ColorPickerDialog
    {

        
        public ColorPickerDialog()
            : base(700)
        {
            this.InitializeComponent();

           Color.SelectedColorChanged += (object sender, EventArgs args) =>
            {
                MainGridLayout.Background = Color.SelectedColor;
            };
        }

        public Color BorderColor { get; set; }
    }
}
