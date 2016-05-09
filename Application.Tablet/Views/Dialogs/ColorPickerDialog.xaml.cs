using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using IndiaRose.Interfaces;
using SharpDX.Direct2D1.Effects;
using Storm.Mvvm.Inject;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Application.Tablet.Views.Dialogs
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ColorPickerDialog
    {
        public IScreenService ScreenService => LazyResolver<IScreenService>.Service;

        public ColorPickerDialog()
            : base((int) Window.Current.Bounds.Height - (int)Window.Current.Bounds.Height *10/100)
        {
            this.InitializeComponent();

           Color.SelectedColorChanged += (object sender, EventArgs args) =>
            {
                MainGridLayout.Background = Color.SelectedColor;
            };
            
            Window.Current.SizeChanged += (sender, args) =>
            {
                Width = ScreenService.Width;
                Height = ScreenService.Height - (ScreenService.Height*10/100);
            };
        }

        public Color BorderColor { get; set; }
    }
}
