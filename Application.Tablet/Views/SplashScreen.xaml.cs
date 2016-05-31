﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Storm.Mvvm;
using IndiaRose.Business.ViewModels;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Application.Tablet.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class SplashScreen
    {
        private const string TILE_ID_USER = "UserPage";
        private const string TILE_ID_ADMIN = "AdminPage";
        private string _tileId;

        public SplashScreen()
        {
            InitializeComponent();

            Loaded += SplashScreen_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
                _tileId = e.Parameter.ToString();

            if (_tileId == TILE_ID_USER)
            {
                DataContext = new SplashScreenViewModel(SplashScreenViewModel.LaunchingType.User); // User
            }
            else
            {
                ImageLogo.Source = new BitmapImage(new Uri("ms-appx:///Assets/logo_admin.png"));
                DataContext = new SplashScreenViewModel(SplashScreenViewModel.LaunchingType.Admin); // Admin
            }
        }

        async void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            //initialisation du second point d'entrée de l'application (partie user)
            var secondaryTileId = TILE_ID_USER;
            if (!SecondaryTile.Exists(secondaryTileId))
            {
                Uri square150x150Logo = new Uri("ms-appx:///Assets/150winUser.png");
                string tileActivationArguments = secondaryTileId + " was pinned at = " + DateTime.Now.ToLocalTime();
                string displayName = "IndiaRose";

                TileSize newTileDesiredSize = TileSize.Square150x150;

                SecondaryTile secondaryTile = new SecondaryTile(secondaryTileId,
                                                                displayName,
                                                                tileActivationArguments,
                                                                square150x150Logo,
                                                                newTileDesiredSize);
                
                secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;
                secondaryTile.VisualElements.BackgroundColor = Colors.White;
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                await secondaryTile.RequestCreateAsync();
            }

            //initialisation du second point d'entrée de l'application (partie admin)
            secondaryTileId = TILE_ID_ADMIN;
            if (!SecondaryTile.Exists(secondaryTileId))
            {
                Uri square150x150Logo = new Uri("ms-appx:///Assets/150winAdmin.png");
                string tileActivationArguments = secondaryTileId + " was pinned at = " + DateTime.Now.ToLocalTime();
                string displayName = "IndiaRose Administrateur";

                TileSize newTileDesiredSize = TileSize.Square150x150;

                SecondaryTile secondaryTile = new SecondaryTile(secondaryTileId,
                                                                displayName,
                                                                tileActivationArguments,
                                                                square150x150Logo,
                                                                newTileDesiredSize);
                
                secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;
                secondaryTile.VisualElements.BackgroundColor = Colors.White;
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                await secondaryTile.RequestCreateAsync();
            }
        }
    }
}
