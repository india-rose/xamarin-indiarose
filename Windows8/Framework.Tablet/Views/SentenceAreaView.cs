
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Framework.Converters;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Framework.Views
{
    public class SentenceAreaView : StackPanel
    {
        public event EventHandler CanAddIndiagramsChanged;

        private bool _canAddIndiagrams = true;
        private int _maxNumberOfIndiagrams;
        private readonly Image _playButton;
        private ObservableCollection<IndiagramUIModel> _indiagrams;
        private readonly List<IndiagramView> _indiagramViews = new List<IndiagramView>();
        private readonly ColorStringToSolidColorBrushConverter _colorConverter = new ColorStringToSolidColorBrushConverter();
        private readonly Grid _grid=new Grid();

        public ICommand IndiagramSelectedCommand { get; set; }

        public ICommand ReadCommand { get; set; }

        public ICommand CorrectionCommand { get; set; }

        public ObservableCollection<IndiagramUIModel> Indiagrams
        {
            get { return _indiagrams; }
            set
            {
                if (!Equals(_indiagrams, value))
                {
                    if (_indiagrams != null)
                    {
                        _indiagrams.CollectionChanged -= IndiagramsOnCollectionChanged;
                    }
                    _indiagrams = value;
                    if (_indiagrams != null)
                    {
                        _indiagrams.CollectionChanged += IndiagramsOnCollectionChanged;
                    }
                }
            }
        }

        public bool CanAddIndiagrams
        {
            get { return _canAddIndiagrams; }
            set
            {
                if (_canAddIndiagrams != value)
                {
                    _canAddIndiagrams = value;
                    this.RaiseEvent(CanAddIndiagramsChanged);
                }
            }
        }

        public SentenceAreaView()
        {
            SizeChanged += SentenceAreaView_SizeChanged;

            // Init play button
            _playButton = new Image()
            {
                Source = new BitmapImage(new Uri(LazyResolver<IStorageService>.Service.ImagePlayButtonPath, UriKind.Absolute)),
                Width = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize,
                Height = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize
            };

            _playButton.Tapped += (sender, args) =>
            {
                if (ReadCommand != null && ReadCommand.CanExecute(null))
                {
                    ReadCommand.Execute(null);
                }
            };
        }

        void SentenceAreaView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var oldmaxnumber = _maxNumberOfIndiagrams;
            _maxNumberOfIndiagrams = ((int)ActualWidth / IndiagramView.DefaultWidth) - 1;
            if (_maxNumberOfIndiagrams == oldmaxnumber)
                return;
            _grid.ColumnDefinitions.Clear();
            for (var i = 0; i < _maxNumberOfIndiagrams + 1; i++)
            {
                _grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            Grid.SetColumn(_playButton, _maxNumberOfIndiagrams);
            Children.Add(_grid);
            _grid.Children.Add(_playButton);
            var settings = LazyResolver<ISettingsService>.Service;

            // Init views
            for (var i = 0; i < _maxNumberOfIndiagrams; ++i)
            {
                var view = new IndiagramView()
                {
                    TextColor = (SolidColorBrush)_colorConverter.Convert(settings.TextColor, null, null, "")
                };
                view.Tapped += OnIndiagramTouched;
                Grid.SetColumn(view, i);
                _grid.Children.Add(view);
                _indiagramViews.Add(view);
            }
        }

        private void IndiagramsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (IndiagramUIModel model in e.OldItems)
                {
                    model.ReinforcerStateChanged -= IndiagramReinforcerChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (IndiagramUIModel model in e.NewItems)
                {
                    model.ReinforcerStateChanged += IndiagramReinforcerChanged;
                }
            }

            //refresh everything
            var i = 0;
            for (; i < _indiagrams.Count; ++i)
            {
                _indiagramViews[i].Indiagram = _indiagrams[i].Model;
            }
            for (; i < _indiagramViews.Count; ++i)
            {
                _indiagramViews[i].Indiagram = null;
            }
            CanAddIndiagrams = (_indiagrams.Count < _indiagramViews.Count);
        }

        private void IndiagramReinforcerChanged(object sender, EventArgs eventArgs)
        {
            var uiModel = sender as IndiagramUIModel;

            if (uiModel != null)
            {
                // find the view associated with this model
                var view =
                    _indiagramViews.FirstOrDefault(
                        x => x.Indiagram != null && Indiagram.AreSameIndiagram(x.Indiagram, uiModel.Model));

                if (view == null)
                {
                    LazyResolver<ILoggerService>.Service.Log(
                        "SentenceAreaView : got a reinforcerChangedEvent from a non registered indiagram !",
                        MessageSeverity.Critical);
                    return;
                }

                if (uiModel.IsReinforcerEnabled)
                {
                    view.Background = (SolidColorBrush)_colorConverter.Convert(LazyResolver<ISettingsService>.Service.ReinforcerColor, null, null, "");
                }
                else
                {
                    // set it back to transparent
                    view.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void OnIndiagramTouched(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            var view = sender as IndiagramView;
            if (view != null && view.Indiagram != null && IndiagramSelectedCommand != null &&
                IndiagramSelectedCommand.CanExecute(view.Indiagram))
            {
                IndiagramSelectedCommand.Execute(view.Indiagram);
            }
        }
    }
}


