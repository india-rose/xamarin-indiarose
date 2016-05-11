using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Framework.Tablet.Converters;
using IndiaRose.Business.ViewModels.User;
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
        private int _heightOfBiggerIndiagram = IndiagramView.DefaultHeight;
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

        public ICommand DragStarCommand { get; set; }

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

            AllowDrop = true;

            // Init play button
            _playButton = new Image
            {
                Source =
                    new BitmapImage(new Uri(LazyResolver<IStorageService>.Service.ImagePlayButtonPath, UriKind.Absolute)),
                Width = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize,
                Height = LazyResolver<ISettingsService>.Service.IndiagramDisplaySize,
                IsHoldingEnabled = true,
                //CanDrag=true
            };

            /*_playButton.DragStarting += (sender, args) =>
            {
                if (CorrectionCommand != null && CorrectionCommand.CanExecute(null))
                {
                    CorrectionCommand.Execute(null);
                } 
            };*/

            _playButton.Tapped += (sender, args) =>
            {
                if (ReadCommand != null && ReadCommand.CanExecute(null))
                {
                    ReadCommand.Execute(null);
                }
            };

            DragEnter += (sender, e) =>
            {
                e.AcceptedOperation = LazyResolver<ISettingsService>.Service.IsMultipleIndiagramSelectionEnabled ? DataPackageOperation.Copy : DataPackageOperation.Move;
            };

            Drop += async (sender, e) =>
            {
                if (DragStarCommand!= null && DragStarCommand.CanExecute(null))
                {
                //var indiagram =  e.quelquechose.get..(le truc dans lequel tu as stocker l'indiagram) as Indiagram

                string indID = await e.DataView.GetTextAsync();
                Indiagram indiagram = GetIndiagramById(Int32.Parse(indID), LazyResolver<ICollectionStorageService>.Service.Collection);

                IndiagramUIModel indiaUi = new IndiagramUIModel(indiagram);
                if (CanAddIndiagrams)
                    Indiagrams.Add(indiaUi);
  
                DragStarCommand.Execute(indiagram);

                }

                //todo voir pour le click 
                //todo voir pour le deplacement de l'indiagram
                //todo voir pour la lecture
                //todo voir pour le retour a l'acceuil apres selection
            };
        }

        /// <summary>
        /// Méthode pour rechercher un indiagram dans une ObservableCollection<Indiagram>
        /// Tout en sachant qu'un indiagram peut être une catégorie, qui va elle même contenir une ObservableCollection<Indiagram>
        /// Et ainsi de suite, il fallait donc faire une méthode récursive
        /// Elle n'est pas contre surement pas au bon endroit ; la déplacer dans le CollectionStorageService ?
        /// </summary>
        /// <param name="searchedId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private Indiagram GetIndiagramById(int searchedId, ObservableCollection<Indiagram> list)
        {
            if (list == null)
                return null;

            foreach (var indiagram in list)
            {
                if (indiagram.Id == searchedId)
                {
                    return indiagram;
                }
                if (indiagram.IsCategory && indiagram.HasChildren)
                {
                    Category category = indiagram as Category;
                    Indiagram temp = GetIndiagramById(searchedId, category.Children);
                    if (temp != null)
                        return temp;
                }
            }
            return null;
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
            Children.Remove(_playButton);
            SetColumn(_playButton, _maxNumberOfIndiagrams);
            Children.Add(_playButton);
            var settings = LazyResolver<ISettingsService>.Service;

            // Init views
            for (var i = 0; i < _maxNumberOfIndiagrams; ++i)
            {
                var view = new IndiagramView(false)
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
            _heightOfBiggerIndiagram = IndiagramView.DefaultHeight;

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

            CheckViewsHeight();
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

            if (view == null)
                return;

            IndiagramUIModel ind = _indiagrams.FirstOrDefault(x => Indiagram.AreSameIndiagram(x.Model, view.Indiagram));

            if (view.Indiagram != null && IndiagramSelectedCommand != null && IndiagramSelectedCommand.CanExecute(ind))
            {
                IndiagramSelectedCommand.Execute(ind);
            }
        }

        /// <summary>
        /// Vérifie la hauteur de l'Indiagram qui a le texte le plus long (au cas où ce dernier est sur plusieurs lignes)
        /// Permet d'aligner tous les Indiagrams par rapport au plus gros, et d'éviter que le renforceur prenne toute la hauteur
        /// La hauteur l'actuel plus gros indiagram est stockée dans _heightOfBiggerIndiagram
        /// </summary>
        private void CheckViewsHeight()
        {
            foreach (var view in _indiagramViews)
            {
                if (view != null && view.Indiagram != null)
                {
                    int h = IndiagramView.DefaultHeight;
                    int w = IndiagramView.DefaultWidth;
                    int length = view.Indiagram.Text.Length;
                    int fontSize = LazyResolver<ISettingsService>.Service.FontSize;
                    int n = length / (w / fontSize);
                    h += n * fontSize;
                    // En théorie, il faudrait utiliser h += --n * fontSize, mais on ne peut pas à cause de l'indiagram lieux/page2/parc d'attraction
                    // Du coup on a une ligne en trop sur la majorité des indiagrams, à régler...

                    if (h > _heightOfBiggerIndiagram)
                        _heightOfBiggerIndiagram = h;
                }
            }

            foreach (var view in _indiagramViews)
            {
                if (view != null && view.Indiagram != null)
                    view.Height = _heightOfBiggerIndiagram;
            }
        }
    }
}


