using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Storm.Mvvm;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Application.Tablet.Views.Admin
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : MvvmPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //initialisation du second point d'entrée de l'application (partie admin)
            /*var secondaryTileId = "AdminPage";
            if (!SecondaryTile.Exists(secondaryTileId))
            {
                Uri square150x150Logo = new Uri("ms-appx:///Assets/150winAdmin.png");
                Uri square30x30Logo = new Uri("ms-appx:///Assets/30winAdmin.png");
                string tileActivationArguments = secondaryTileId + " was pinned at = " + DateTime.Now.ToLocalTime();
                string displayName = "IndiaRose.Admin";

                TileSize newTileDesiredSize = TileSize.Square150x150;

                SecondaryTile secondaryTile = new SecondaryTile(secondaryTileId,
                                                                displayName,
                                                                tileActivationArguments,
                                                                square150x150Logo,
                                                                newTileDesiredSize);

                secondaryTile.VisualElements.Square30x30Logo = square30x30Logo;
                secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;
                secondaryTile.VisualElements.BackgroundColor = Colors.CornflowerBlue;
                await secondaryTile.RequestCreateAsync();
            }*/
        }
    }
}
