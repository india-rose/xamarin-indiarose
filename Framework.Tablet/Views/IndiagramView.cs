using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace Framework.Tablet.Views
{
    public class IndiagramView : StackPanel
    {
        protected static ISettingsService SettingsService => LazyResolver<ISettingsService>.Service;

        private readonly TextBlock _textBlock;
        private readonly Image _image;
        /// <summary>
        /// Si l'Indiagram n'a pas d'Image, on affiche un carré rouge
        /// </summary>
        private readonly StackPanel _redRect;

        #region TextColor
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
                "TextColor", typeof(SolidColorBrush), typeof(IndiagramView), new PropertyMetadata(default(SolidColorBrush), RefreshColor));

        private static void RefreshColor(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var indiaView = dependencyObject as IndiagramView;
            if (indiaView != null) indiaView.RefreshColor();
        }

        /// <summary>
        /// Couleur du texte de l'Indiagram
        /// </summary>
        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }
        #endregion

        #region Indiagram

        public static readonly DependencyProperty IndiagramProperty = DependencyProperty.Register(
                       "Indiagram", typeof(Indiagram), typeof(IndiagramView), new PropertyMetadata(default(Indiagram), RefreshDisplay));

        protected static void RefreshDisplay(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var indiaView = dependencyObject as IndiagramView;
            indiaView?.RefreshDisplay();
        }

        #endregion

        /// <summary>
        /// Indiagram à afficher
        /// </summary>
        public Indiagram Indiagram
        {
            get { return (Indiagram)GetValue(IndiagramProperty); }
            set
            {
                SetValue(IndiagramProperty, value);
                if (value != null)
                {
                    CanDrag = !value.IsCategory && LazyResolver<ISettingsService>.Service.IsDragAndDropEnabled;
                }

            }
        }

        public IndiagramView()
        {
            Orientation = Orientation.Vertical;
            var indiaSize = SettingsService.IndiagramDisplaySize;
            var margin = indiaSize / 10;
            Width = indiaSize + 2 * margin;
            Children.Clear();
            _image = new Image
            {
                Margin = new Thickness(0, margin, 0, 0),
                Height = SettingsService.IndiagramDisplaySize,
                Width = SettingsService.IndiagramDisplaySize
            };

            _textBlock = new TextBlock
            {
                Margin = new Thickness(margin, 0, margin, 0),
                FontSize = SettingsService.FontSize,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new FontFamily(SettingsService.FontName)
            };
            _redRect = new StackPanel
            {
                Margin = new Thickness(0, margin, 0, 0),
                Height = SettingsService.IndiagramDisplaySize,
                Width = SettingsService.IndiagramDisplaySize,
                Background = new SolidColorBrush(Colors.Red)
            };

            DragStarting += (sender, args) =>
            {
                args.Data.Properties.Add("item",Indiagram);
            };

            Children.Insert(0, _image);
            Children.Add(_textBlock);
        }

        private void RefreshColor()
        {
            _textBlock.Foreground = TextColor;
        }

        protected virtual void RefreshDisplay()
        {
            if (Indiagram == null)
            {
                Children.Clear();
                return;
            }

            try
            {
                Children.Remove(Children[0]);
            }
            catch (ArgumentException)
            {
            }

            if (!string.IsNullOrEmpty(Indiagram.ImagePath))
            {
                //si l'Indiagram a une image
                if (Children[0] != _image)
                {
                    Children.Insert(0, _image);

                    if (Children[1] == null)
                        Children.Insert(1, _textBlock);
                }
                _image.Source = new BitmapImage(new Uri(Indiagram.ImagePath, UriKind.Absolute));
            }
            else
            {
                //si l'indiagram n'a pas d'image
                Children.Insert(0, _redRect);

                if (Children[1] == null)
                    Children.Insert(1, _textBlock);
            }
            if (!Indiagram.IsEnabled)
            {
                _image.Opacity = 0.5;
                _redRect.Opacity = 0.5;
            }
            else
            {
                _image.Opacity = 1.0;
                _redRect.Opacity = 1.0;
            }
            if (!string.IsNullOrEmpty(Indiagram.Text))
                _textBlock.Text = Indiagram.Text;

            //if (Indiagram.HasChildren)
            //_image.CanDrag = false;

        }

        /// <summary>
        /// Largeur attendu pour la Vue
        /// </summary>
        public static int DefaultWidth => (int)(SettingsService.IndiagramDisplaySize * 1.2);

        /// <summary>
        /// Hauteur attendu pour la Vue (avec une seul ligne de texte)
        /// </summary>
        public static int DefaultHeight => (int)(SettingsService.IndiagramDisplaySize * 1.2 + SettingsService.FontSize);
    }
}