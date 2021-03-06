﻿using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    /// <summary>
    /// Affiche la barre de titre de la partie utilisateur
    /// </summary>
    public class TitleBarView : Canvas
    {
        /// <summary>
        /// Image de la Categorie courante
        /// </summary>
        private readonly Image _imagecategory;
        /// <summary>
        /// Texte de la Catégorie courante
        /// </summary>
        private readonly TextBlock _textblock;
        /// <summary>
        /// Carré rouge, servant lorsque la Catégorie courante n'a pas d'image
        /// </summary>
        private readonly StackPanel _redRect;

        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(
            "Category", typeof(Category), typeof(TitleBarView), new PropertyMetadata(default(Category), Refresh));

        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TitleBarView;
            if (view != null) view.Refresh();
        }

        private void Refresh()
        {
            _textblock.Text = Category.Text;
            //pourquoi ne pas sortir la suppression ?
            if (Category.ImagePath == null)
            {
                //si la catégorie n'a pas d'image on met le rectangle rouge
                try
                {
                    Children.RemoveAt(0);
                }
                catch (ArgumentException) { }
                Children.Insert(0, _redRect);
            }
            else
            {
                try
                {
                    Children.RemoveAt(0);
                }
                catch (ArgumentException) { }
                _imagecategory.Source = new BitmapImage(new Uri(Category.ImagePath, UriKind.Absolute));
                Children.Insert(0, _imagecategory);
            }
        }

        public Category Category
        {
            get { return (Category)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public TitleBarView()
        {
            var settingsService = LazyResolver<ISettingsService>.Service;
            _textblock = new TextBlock
            {
                FontSize = 20,
                Foreground = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };

            _redRect = new StackPanel
            {
                Height = 60,
                Width = 60,
                Background = new SolidColorBrush(Colors.Red)
            };

            _imagecategory = new Image
            {
                Height = 60,
                Width = 60
            };

            _textblock.Measure(new Size(0, 0));

            Height = 60;
            SetLeft(_textblock, _redRect.Width);
            SetTop(_textblock, (Height - _textblock.ActualHeight) / 2);
            Background = new SolidColorBrush(Colors.White);

            const string sourcelogo = "ms-appx:///Assets/logoIndiaRose.png";
            var logo = new Image
            {
                Source = new BitmapImage(new Uri(sourcelogo)),
                Width = 256
            };
            SetLeft(logo, LazyResolver<IScreenService>.Service.Width - logo.Width);

            Children.Insert(0, _imagecategory);
            Children.Insert(1, _textblock);
            Children.Insert(2, logo);

        }
    }
}
