using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

namespace Application.Tablet.Views.Dialogs
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ExploreCollectionCategoryWithoutChildrenDialog 
    {
        public IScreenService ScreenService => LazyResolver<IScreenService>.Service;

        public ExploreCollectionCategoryWithoutChildrenDialog() : base((int)Window.Current.Bounds.Height - (int)Window.Current.Bounds.Height * 55 / 100)
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
