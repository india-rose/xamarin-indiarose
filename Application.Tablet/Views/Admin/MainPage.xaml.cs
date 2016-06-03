using System;
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
using Windows.UI.Xaml.Navigation;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Inject;

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
            //Loaded += MainPage_Loaded;

            /*SizeChanged += (sender, args) =>
            {
                Debug.WriteLine(LazyResolver<IScreenService>.Service.Height + " x " + LazyResolver<IScreenService>.Service.Width);
                LinkIndiaRose.FontSize = LazyResolver<IScreenService>.Service.Height / 27;
            };*/
        }

        /*async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }*/
    }
}
