using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace Framework.Tablet.Views
{
    /// <summary>
    /// Sert à l'affichage de la collection
    /// </summary>
    public class IndiagramBrowserView : StackPanel
    {
        #region Private fields
        private int _columnCount;
        private int _lineCount;
        private IndiagramView[][] _displayableViews;
        private readonly Image _nextButton;
        private readonly Grid _grid;
        #endregion

        public event EventHandler CountChanged;
        #region DependencyProperty

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
                "Count", typeof(int), typeof(IndiagramBrowserView), new PropertyMetadata(default(int), CountChangedRaising));

        private static void CountChangedRaising(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as IndiagramBrowserView;
            if (view != null) view.RaiseCountChanged();
        }

        public void RaiseCountChanged()
        {
            if (CountChanged != null)
            {
                CountChanged(this, null);
            }
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
                "Offset", typeof(int), typeof(IndiagramBrowserView), new PropertyMetadata(default(int), Refresh));

        public static readonly DependencyProperty IndiagramsProperty = DependencyProperty.Register(
                "Indiagrams", typeof(List<Indiagram>), typeof(IndiagramBrowserView), new PropertyMetadata(default(List<Indiagram>), Refresh));

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
                "TextColor", typeof(SolidColorBrush), typeof(IndiagramBrowserView), new PropertyMetadata(default(SolidColorBrush), RefreshTextColor));

        public static readonly DependencyProperty IndiagramSelectedProperty = DependencyProperty.Register(
                  "IndiagramSelected", typeof(ICommand), typeof(IndiagramBrowserView), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(
                   "NextCommand", typeof(ICommand), typeof(IndiagramBrowserView), new PropertyMetadata(default(ICommand)));

        #endregion

        #region Public Properties
        /// <summary>
        /// Nombre d'Indiagram à afficher
        /// </summary>
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        /// <summary>
        /// Numéro du premier Indiagram à afficher
        /// </summary>
        public int Offset
        {
            get { return (int)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        /// <summary>
        /// Liste des Indiagrams à afficher
        /// </summary>
        public List<Indiagram> Indiagrams
        {
            get { return (List<Indiagram>)GetValue(IndiagramsProperty); }
            set { SetValue(IndiagramsProperty, value); }
        }

        /// <summary>
        /// Couleur des textes des Indiagrams
        /// </summary>
        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Commande lorsqu'un indiagram est sélectionné
        /// </summary>
        public ICommand IndiagramSelected
        {
            get { return (ICommand)GetValue(IndiagramSelectedProperty); }
            set { SetValue(IndiagramSelectedProperty, value); }
        }

        /// <summary>
        /// Commande lorsque le bouton suivant est sélectionné
        /// </summary>
        public ICommand NextCommand
        {
            get { return (ICommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }

        /// <summary>
        /// Permet de savoir si les indiagrams contenus doivent être draggable ou pas.
        /// Propriété publique, même si à l'heure actuelle elle n'a pas besoin de l'être, cela permet de la binder facilement.
        /// </summary>
        public bool DraggableIndiagrams { get; set; }

        #endregion

        public IndiagramBrowserView()
        {
            DraggableIndiagrams = true;
            var indiaSize = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize;
            var margin = indiaSize / 10;
            _nextButton = new Image()
            {
                VerticalAlignment = VerticalAlignment.Top,
                Height = indiaSize,
                Width = indiaSize,
                Margin = new Thickness(margin, margin, margin, 0),
                Source =
                    new BitmapImage(new Uri(LazyResolver<IStorageService>.Service.ImageNextArrowPath, UriKind.Absolute))
            };
            _grid = new Grid();
            _nextButton.Tapped += _nextButton_Tapped;
            SizeChanged += OnSizeChanged;

        }

        private void _nextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NextCommand != null && NextCommand.CanExecute(null))
                NextCommand.Execute(null);
        }

        #region Private static callback for DependencyProperty
        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var indiaBrowserView = d as IndiagramBrowserView;
            if (indiaBrowserView != null) indiaBrowserView.RefreshDisplay();
        }

        private static void RefreshTextColor(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var indiaBrowserView = dependencyObject as IndiagramBrowserView;
            if (indiaBrowserView != null) indiaBrowserView.RefreshTextColor();
        }
        #endregion

        /// <summary>
        /// Raffraichit la couleur du texte des Indiagrams
        /// </summary>
        private void RefreshTextColor()
        {
            if (_displayableViews == null)
                return;
            foreach (var cell in _displayableViews.SelectMany(line => line))
            {
                cell.TextColor = TextColor;
            }
        }

        /// <summary>
        /// Réinitialise la taille de la grille d'affichage
        /// </summary>
        private void ResetGrid()
        {
            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();

            for (int i = 0; i < _columnCount; i++)
            {
                _grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            for (int i = 0; i < _lineCount; i++)
            {
                _grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }
            Grid.SetColumn(_nextButton, _columnCount - 1);
            Grid.SetRow(_nextButton, 0);
            Children.Add(_grid);
        }

        /// <summary>
        /// Callback lorsque la taille de la Vue change
        /// Lance le raffraichissement
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Reset())
                RefreshDisplay();
        }

        /// <summary>
        /// Reset l'affichage de la collection
        /// </summary>
        /// <returns>Retourne faux lorsque le reset ne changerait rien</returns>
        private bool Reset()
        {
            int newColumnCount = (int)(ActualWidth / IndiagramView.DefaultWidth);
            int newLineCount = (int)(ActualHeight / IndiagramView.DefaultHeight);
            if ((int)ActualHeight == 0)
                newLineCount = (int)(Height / IndiagramView.DefaultHeight);

            if (newColumnCount != _columnCount || newLineCount != _lineCount)
            {
                _columnCount = newColumnCount;
                _lineCount = newLineCount;
            }
            else
            {
                return false;
            }

            if (_displayableViews != null)
            {
                Children.Clear();
                _displayableViews = null;
            }
            _displayableViews = new IndiagramView[_lineCount][];
            for (int line = 0; line < _lineCount; ++line)
            {
                _displayableViews[line] = new IndiagramView[_columnCount - (line == 0 ? 1 : 0)];
                for (int column = 0; column < _displayableViews[line].Length; ++column)
                {
                    var view = new IndiagramView { TextColor = TextColor };
                    view.Tapped += view_Tapped;
                    _displayableViews[line][column] = view;
                }
            }
            ResetGrid();
            return true;
        }

        /// <summary>
        /// Callback lorsqu'un Indiagram a été sélectionné
        /// </summary>=
        void view_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var senderView = sender as IndiagramView;
            if (senderView == null)
            {
                return;
            }
            var indiagram = senderView.Indiagram;
            if (IndiagramSelected != null && IndiagramSelected.CanExecute(indiagram))
            {
                IndiagramSelected.Execute(indiagram);
                RefreshDisplay();
            }
        }

        /// <summary>
        /// Raffraichit l'affichage
        /// </summary>
        private void RefreshDisplay()
        {
            if (Indiagrams == null || _displayableViews == null || _lineCount == 0 || _columnCount == 0)
            {
                return;
            }
            _grid.Children.Clear();
            _grid.Children.Add(_nextButton);
            IndiagramView oldview = null;
            List<Indiagram> toDisplay = Indiagrams.Where((o, i) => i >= Offset).ToList();
            int displayCount = 0;
            int index = 0;
            bool stop = false;
            for (int line = 0; line < _lineCount; ++line)
            {
                int lineCount = 0;
                for (int column = 0; column < _displayableViews[line].Length; ++column)
                {
                    if (index >= toDisplay.Count)
                    {
                        stop = true;
                        break;
                    }
                    IndiagramView view = _displayableViews[line][column];
                    view.Indiagram = toDisplay[index++];
                    lineCount++;
                    Grid.SetColumn(view, column);
                    Grid.SetRow(view, line);
                    _grid.Children.Add(view);
                    if (oldview != null) oldview.SizeChanged -= LastLineVerification;
                    view.SizeChanged += LastLineVerification;
                    oldview = view;
                }
                displayCount += lineCount;
                if (stop)
                    break;
            }
            Count = displayCount;
        }

        /// <summary>
        /// Vérifie que la dernière ligne ne dépasse pas du bord de l'écran
        /// </summary>
        private void LastLineVerification(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var view = sender as IndiagramView;
            if (view != null) view.SizeChanged -= LastLineVerification;
            var totalHeight = 0.0;
            foreach (var line in _displayableViews)
            {
                var lineHeight = 0.0;
                foreach (var cell in line)
                {
                    if (cell.Indiagram != null)
                        if (cell.ActualHeight > lineHeight)
                            lineHeight = cell.ActualHeight;
                }
                totalHeight += lineHeight;
            }
            if (totalHeight > DesiredSize.Height)
            {
                var displayCount = 0;
                {
                    foreach (var cell in _displayableViews[_lineCount - 1])
                    {
                        _grid.Children.Remove(cell);
                        if (cell.Indiagram != null)
                        {
                            displayCount++;
                        }
                    }
                    Count -= displayCount;
                }
            }
        }
    }
}
