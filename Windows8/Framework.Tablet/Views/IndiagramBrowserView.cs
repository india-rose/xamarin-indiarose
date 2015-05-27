using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation.Diagnostics;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class IndiagramBrowserView : Panel
    {
        #region Private fields
        private int _columnCount;
        private int _lineCount;
        private Image[][] _displayableViews;
        private readonly Image _nextButton;
        private readonly int _indiaSize;
        private int _margin;
        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
                "Count", typeof(int), typeof(IndiagramBrowserView), new PropertyMetadata(default(int)));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
                   "Offset", typeof(int), typeof(IndiagramBrowserView), new PropertyMetadata(default(int)));
        public static readonly DependencyProperty IndiagramsProperty = DependencyProperty.Register(
                   "Indiagrams", typeof(List<Indiagram>), typeof(IndiagramBrowserView), new PropertyMetadata(default(List<Indiagram>)));
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(uint), typeof(IndiagramBrowserView), new PropertyMetadata(default(uint)));
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

        public uint TextColor
        {
            get { return (uint)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
                RefreshTextColor();
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
            _indiaSize = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize;
            int margin = _indiaSize / 10;
            _nextButton = new Image()
            {
                Height = _indiaSize,
                Width = _indiaSize,
                Margin = new Thickness(margin, margin, margin, 0),
                Source =
                    new BitmapImage(new Uri(LazyResolver<IStorageService>.Service.ImageNextArrowPath, UriKind.Absolute))
            };
            Loaded += Init;
            //SizeChanged += OnSizeChanged;

        }

        private void Init(object sender, RoutedEventArgs e)
        {
            var g = new Grid();
            for (int i = 0; i < 10; i++)
            {
                    g.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });
                    g.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = GridLength.Auto
                    });
            }
            g.Children.Add(_nextButton);
            Grid.SetColumn(_nextButton,2);
            Grid.SetRow(_nextButton, 2);
            var t = new Image()
            {
                Height = _indiaSize,
                Width = _indiaSize,
                Source =
                    new BitmapImage(new Uri(LazyResolver<IStorageService>.Service.ImageCorrectionPath, UriKind.Absolute))
            };
            g.Children.Add(t);

            Grid.SetColumn(t, 3);
            Grid.SetRow(t, 3);
            Children.Insert(0, g);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Reset())
                RefreshDisplay();
        }

        private bool Reset()
        {
            int newColumnCount = (int)(ActualHeight / (_indiaSize + (_indiaSize / 10)));
            int newLineCount = (int)(ActualWidth / (_indiaSize + (_indiaSize / 10)));

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
            _displayableViews = new Image[_lineCount][];
            for (int line = 0; line < _lineCount; ++line)
            {
                _displayableViews[line] = new Image[_columnCount - ((line == 0) ? 1 : 0)];
                for (int column = 0; column < _displayableViews[line].Length; ++column)
                {
                    var view = new Image()
                    {
                        Height = _indiaSize,
                        Width = _indiaSize
                    };
                    _displayableViews[line][column] = view;
                }
            }
            return true;
        }
        private void RefreshTextColor()
        {
            throw new System.NotImplementedException();
        }

        private void RefreshDisplay(object sender = null, RoutedEventArgs e = null)
        {
            if (Indiagrams == null || _displayableViews == null || _lineCount == 0 || _columnCount == 0)
            {
                return;
            }
            Children.Clear();
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
                    int oldindex = index;
                    Image view = _displayableViews[line][column];
                    view.Source = new BitmapImage(new Uri(toDisplay[(index++) - (line > 0 ? 1 : 0)].ImagePath, UriKind.Absolute));
                    view.Margin = new Thickness(_margin, _margin, 0, 0);
                    lineCount++;
                    Children.Insert(oldindex, view);
                    if (lineHeight < view.Height)
                        lineHeight = (int)view.Height;
                }
                if (line == 0)
                    Children.Insert(index + 1, _nextButton);
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
