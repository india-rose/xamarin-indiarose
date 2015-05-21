using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace IndiaRose.Application.Views.Dialogs
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ColorPickerDialog
    {
        public ColorPickerDialog() : base (300)
        {
            this.InitializeComponent();
            var colors = typeof(Colors).GetTypeInfo().DeclaredProperties;
            foreach (var item in colors)
            {
                if (CbBorderColor.Items != null) CbBorderColor.Items.Add(item);
            }
        }
        private void cbBorderColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbBorderColor.SelectedIndex != -1)
            {
                var pi = CbBorderColor.SelectedItem as PropertyInfo;
                BorderColor = (Color)pi.GetValue(null);
            }
        }

        public Color BorderColor { get; set; }
    }
}
