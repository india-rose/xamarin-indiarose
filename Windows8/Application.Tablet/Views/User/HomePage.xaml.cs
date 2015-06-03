using System;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace IndiaRose.Application.Views.User
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class HomePage
    {
        private ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }
        public HomePage()
        {
            if (SettingsService.IsLoaded)
                SettingsService_Loaded();
            else
                SettingsService.Loaded += SettingsService_Loaded;
        }

        void SettingsService_Loaded(object sender = null, EventArgs e = null)
        {
            InitializeComponent();
        }
    }
}
