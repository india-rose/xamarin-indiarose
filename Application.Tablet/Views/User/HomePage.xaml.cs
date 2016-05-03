using System;
using Windows.UI;
using Windows.UI.StartScreen;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Application.Tablet.Views.User
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

            DataContextChanged += HomePage_DataContextChanged;
            //Loaded += HomePage_Loaded;
        }

        /*async void HomePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }*/

        void HomePage_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            ViewModelBase viewModel = DataContext as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.OnNavigatedTo(new NavigationArgs(NavigationArgs.NavigationMode.New), null);
            }
        }

        void SettingsService_Loaded(object sender = null, EventArgs e = null)
        {
            InitializeComponent();
        }
    }
}
