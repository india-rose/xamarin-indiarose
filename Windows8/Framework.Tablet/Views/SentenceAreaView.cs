
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Android.Support.V4.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Framework.Converters;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using Storm;


namespace IndiaRose.Framework.Views
{
    public class SentenceAreaView : StackPanel
    {
        public event EventHandler CanAddIndiagramsChanged;

        private bool _canAddIndiagrams = true;
        private int _maxNumberOfIndiagrams;
        private IndiagramView _playButton;
        private ObservableCollection<IndiagramUIModel> _indiagrams;
        private readonly List<IndiagramView> _indiagramViews = new List<IndiagramView>();

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
            _maxNumberOfIndiagrams = LazyResolver<IScreenService>.Service.Width/IndiagramView.DefaultWidth - 1;
            ISettingsService settings = LazyResolver<ISettingsService>.Service;
            var colorConverter = new ColorStringToSolidColorBrushConverter();

            // Init views
            for (int i = 0; i < _maxNumberOfIndiagrams; ++i)
            {
                IndiagramView view = new IndiagramView()
                {
                    TextColor = (SolidColorBrush) colorConverter.Convert(settings.TextColor,null,null,"")
                };
            }


            // Init play button
            _playButton = new IndiagramView()
            {
                Indiagram = new Indiagram()
                {
                    Text = "play",
                    ImagePath = LazyResolver<IStorageService>.Service.ImagePlayButtonPath
                }
            };

            _playButton.Tapped += (sender, args) =>
            {
                        if (CorrectionCommand != null && CorrectionCommand.CanExecute(null))
                        {
                            CorrectionCommand.Execute(null);
                        }
                    else if (ReadCommand != null && ReadCommand.CanExecute(null))
                    {
                        ReadCommand.Execute(null);
                    }
            };
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
            int i = 0;
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
            IndiagramUIModel uiModel = sender as IndiagramUIModel;

            if (uiModel != null)
            {
                // find the view associated with this model
                IndiagramView view =
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
                    view.Background = (new ColorStringToIntConverter().Convert(
                        LazyResolver<ISettingsService>.Service.ReinforcerColor));
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
                IndiagramView view = sender as IndiagramView;
                if (view != null && view.Indiagram != null && IndiagramSelectedCommand != null &&
                    IndiagramSelectedCommand.CanExecute(view.Indiagram))
                {
                    IndiagramSelectedCommand.Execute(view.Indiagram);
                }
            }
        }
    }


