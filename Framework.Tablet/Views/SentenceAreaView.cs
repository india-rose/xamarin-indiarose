using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Framework.Tablet.Converters;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Framework.Tablet.Views
{
    /// <summary>
    /// Affichage des Indiagrams devant être lu dans la partie utilisateur
    /// </summary>
    public class SentenceAreaView : Grid
    {
        /// <summary>
        /// Événement indiquant qu'il y a un changement de la propriété CannAddIndiagrams
        /// </summary>
        /// <see cref="CanAddIndiagrams"/>
        public event EventHandler CanAddIndiagramsChanged;

        #region Private fields
        private bool _canAddIndiagrams = true;
        private int _maxNumberOfIndiagrams;
        private readonly Image _playButton;
        private ObservableCollection<IndiagramUIModel> _indiagrams;
        private readonly List<IndiagramView> _indiagramViews = new List<IndiagramView>();
        private readonly ColorStringToSolidColorBrushConverter _colorConverter = new ColorStringToSolidColorBrushConverter();
        #endregion

        #region Properties
        /// <summary>
        /// Commande lorsqu'un Indiagram a été sélectionné
        /// </summary>
        public ICommand IndiagramSelectedCommand { get; set; }

        /// <summary>
        /// Commande lorsque le bouton lecture a été sélectionné
        /// </summary>
        public ICommand ReadCommand { get; set; }

        /// <summary>
        /// Commande lorsque le mode Correction a été demandé
        /// </summary>
        public ICommand CorrectionCommand { get; set; }

        /// <summary>
        /// Indiagrams devant être affichés
        /// </summary>
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

        /// <summary>
        /// Booléen indiquant s'il reste de la place dans la vue pour ajouter de nouveau Indiagrams
        /// </summary>
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
        #endregion

        public SentenceAreaView()
        {
            SizeChanged += SentenceAreaView_SizeChanged;

            // Init play button
            _playButton = new Image
            {
                Source = new BitmapImage(new Uri(LazyResolver<IStorageService>.Service.ImagePlayButtonPath, UriKind.Absolute)),
                Width = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize,
                Height = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize
            };

            _playButton.IsRightTapEnabled = true;
            _playButton.RightTapped += (sender, args) =>
            {
                if (CorrectionCommand != null && CorrectionCommand.CanExecute(null))
                {
                    CorrectionCommand.Execute(null);
                } 
            };
            _playButton.Tapped += (sender, args) =>
            {
                if (ReadCommand != null && ReadCommand.CanExecute(null))
                {
                    ReadCommand.Execute(null);
                }
            };
        }

        /// <summary>
        /// Callback lorsque la taille de la vue a changé
        /// Réinitialise l'affichage de la vue
        /// </summary>
        void SentenceAreaView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var oldmaxnumber = _maxNumberOfIndiagrams;
            _maxNumberOfIndiagrams = ((int)ActualWidth / IndiagramView.DefaultWidth) - 1;
            if (_maxNumberOfIndiagrams == oldmaxnumber)
                return;
            ColumnDefinitions.Clear();
            for (var i = 0; i < _maxNumberOfIndiagrams + 1; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            SetColumn(_playButton, _maxNumberOfIndiagrams);
            Children.Clear();
            Children.Add(_playButton);
            var settings = LazyResolver<ISettingsService>.Service;

            // Init views
            for (var i = 0; i < _maxNumberOfIndiagrams; ++i)
            {
                var view = new IndiagramView
                {
                    TextColor = (SolidColorBrush)_colorConverter.Convert(settings.TextColor, null, null, "")
                };
                view.Tapped += OnIndiagramTouched;
                SetColumn(view, i);
                Children.Add(view);
                _indiagramViews.Add(view);
            }
        }

        /// <summary>
        /// Callback lorsque la collection à afficher a changé
        /// </summary>
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

        /// <summary>
        /// Callback lorsque le renforçateur d'un Indiagram change
        /// </summary>
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
                    view.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        /// <summary>
        /// Callback lorsqu'un Indiagram est sélectionné
        /// </summary>
        private void OnIndiagramTouched(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            var view = sender as IndiagramView;
            IndiagramUIModel ind = _indiagrams.FirstOrDefault(x => Indiagram.AreSameIndiagram(x.Model, view.Indiagram));

            if (view != null && view.Indiagram != null && IndiagramSelectedCommand != null &&
                IndiagramSelectedCommand.CanExecute(ind))
            {
                IndiagramSelectedCommand.Execute(ind);
            }
        }
    }
}


