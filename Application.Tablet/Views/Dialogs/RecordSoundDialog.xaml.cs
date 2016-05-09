// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace Application.Tablet.Views.Dialogs
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecordSoundDialog
    {
        public IScreenService ScreenService => LazyResolver<IScreenService>.Service;

        public RecordSoundDialog()
            : base((int) Window.Current.Bounds.Height - (int) Window.Current.Bounds.Height*55/100)
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += (sender, args) =>
            {
                Width = ScreenService.Width;
                Height = ScreenService.Height - (ScreenService.Height * 55 / 100);
            };
        }
    }
}
