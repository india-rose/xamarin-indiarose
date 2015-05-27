using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IndiaRose.Business.ViewModels;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace IndiaRose.Application.Views.Dialogs
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
            Color.colorChanged += (object sender, EventArgs args) =>
            {
                // Main Grid 
                MainGridLayout.Background = new SolidColorBrush(Color.SelectedColor);
                Grid.Background = new SolidColorBrush(Color.SelectedColor);
                var black = new SolidColorBrush(Colors.Black);
                var white = new SolidColorBrush(Colors.White);
                if (TooLight(Color.SelectedColor))
                {
                    Title.Foreground =
                        Expl.Foreground =
                            Ok.Foreground = Ok.BorderBrush = Cancel.Foreground = Cancel.BorderBrush = black;
                }
                else
                {
                    Title.Foreground =
                        Expl.Foreground =
                            Ok.Foreground = Ok.BorderBrush = Cancel.Foreground = Cancel.BorderBrush = white;
                }
            };
        }

        private bool TooLight(Color color)
        {
            if ((color.B > 0x09) && (color.G > 0x09) && (color.R > 0x09))
            {
                return true;
            }
            return false;
        }

        public Color BorderColor { get; set; }
    }
}
