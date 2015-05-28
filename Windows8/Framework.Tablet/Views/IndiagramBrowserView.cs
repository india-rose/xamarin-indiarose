using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation.Diagnostics;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class IndiagramBrowserView : StackPanel
    {
        #region Private fields
        private int _columnCount;
        private int _lineCount;
        private IndiagramView[][] _displayableViews;
        private readonly Image _nextButton;
        private Grid g;
        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
                "Count", typeof(int), typeof(IndiagramBrowserView), new PropertyMetadata(default(int)));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
                   "Offset", typeof(int), typeof(IndiagramBrowserView), new PropertyMetadata(default(int)));
        public static readonly DependencyProperty IndiagramsProperty = DependencyProperty.Register(
                   "Indiagrams", typeof(List<Indiagram>), typeof(IndiagramBrowserView), new PropertyMetadata(default(List<Indiagram>)));
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(SolidColorBrush), typeof(IndiagramBrowserView), new PropertyMetadata(default(SolidColorBrush)));
        public static readonly DependencyProperty IndiagramSelectedProperty = DependencyProperty.Register(
                  "IndiagramSelected", typeof(ICommand), typeof(IndiagramBrowserView), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(
                   "NextCommand", typeof(ICommand), typeof(IndiagramBrowserView), new PropertyMetadata(default(ICommand)));
        #endregion
        #region Public Properties
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public int Offset
        {
            get { return (int)GetValue(OffsetProperty); }
            set
            {
                SetValue(OffsetProperty, value);
                RefreshDisplay();
            }
        }

        public List<Indiagram> Indiagrams
        {
            get { return (List<Indiagram>)GetValue(IndiagramsProperty); }
            set
            {
                SetValue(IndiagramsProperty, value);
                RefreshDisplay();
            }
        }

        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        public ICommand IndiagramSelected
        {
            get { return (ICommand)GetValue(IndiagramSelectedProperty); }
            set { SetValue(IndiagramSelectedProperty, value); }
        }

        public ICommand NextCommand
        {
            get { return (ICommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }
        #endregion

        public IndiagramBrowserView()
            : base()
        {
            var indiaSize = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize;
            var margin = indiaSize / 10;
            _nextButton = new Image()
            {
                Height = indiaSize,
                Width = indiaSize,
                Margin = new Thickness(margin, 0, margin, 0),
                Source =
                    new BitmapImage(new Uri(LazyResolver<IStorageService>.Service.ImageNextArrowPath, UriKind.Absolute))
            };
            _nextButton.Tapped += _nextButton_Tapped;
            SizeChanged += OnSizeChanged;

        }

        void _nextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NextCommand != null && NextCommand.CanExecute(null))
                NextCommand.Execute(null);
        }


        private void ResetGrid()
        {
            g = new Grid();

            for (int i = 0; i < _columnCount; i++)
            {
                g.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            for (int i = 0; i < _lineCount; i++)
            {
                g.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }
            Grid.SetColumn(_nextButton, _columnCount - 1);
            Grid.SetRow(_nextButton, 0);
            Children.Add(g);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Reset())
                RefreshDisplay();
        }

        private bool Reset()
        {
            int newColumnCount = (int)(ActualWidth / IndiagramView.DefaultWidth);
            int newLineCount = (int)(ActualHeight / IndiagramView.DefaultHeight);

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
                _displayableViews[line] = new IndiagramView[_columnCount - ((line == 0) ? 1 : 0)];
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
            }
        }

        private void RefreshDisplay(object sender = null, RoutedEventArgs e = null)
        {
            if (Indiagrams == null || _displayableViews == null || _lineCount == 0 || _columnCount == 0)
            {
                return;
            }
            g.Children.Clear();
            g.Children.Add(_nextButton);
            List<Indiagram> toDisplay = Indiagrams.Where((o, i) => i >= Offset).ToList();
            int displayCount = 0;
            int index = 0;
            int currentHeight = 0;
            bool stop = false;
            for (int line = 0; line < _lineCount; ++line)
            {
                int lineHeight = 0;
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
                    g.Children.Add(view);
                    if (lineHeight < view.Height)
                        lineHeight = (int)view.Height;
                }
                currentHeight += lineHeight;
                if (currentHeight > Height)
                {
                    stop = true;
                    for (int column = 0; column < lineCount; ++column)
                    {
                        Children.Remove(_displayableViews[line][column]);
                    }
                }
                else
                {
                    displayCount += lineCount;
                }
                if (stop)
                    break;
            }
            Count = displayCount;
        }
    }
}
