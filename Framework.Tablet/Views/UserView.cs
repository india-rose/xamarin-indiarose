using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace Framework.Tablet.Views
{
    /// <summary>
    /// Affiche la partie utilisateur
    /// Est composé de 2 parties : IndiagramBrowserView et SentenceAreaView
    /// </summary> 
    /// <see cref="IndiagramBrowserView"/>
    /// <see cref="SentenceAreaView"/>
    public class UserView : StackPanel
    {
        private readonly SentenceAreaView _botScreen;
        private readonly IndiagramBrowserView _topScreen;

        #region Properties

        #region TopBackground
        public static readonly DependencyProperty TopBackgroundProperty = DependencyProperty.Register(
            "TopBackground", typeof(SolidColorBrush), typeof(UserView), new PropertyMetadata(default(SolidColorBrush), RefreshTopBackgroundColor));

        private static void RefreshTopBackgroundColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshTopBackgroundColor();
        }

        private void RefreshTopBackgroundColor()
        {
            _topScreen.Background = TopBackground;
        }

        public SolidColorBrush TopBackground
        {
            get { return (SolidColorBrush)GetValue(TopBackgroundProperty); }
            set { SetValue(TopBackgroundProperty, value); }
        }
        #endregion

        #region BotBackgorund
        public static readonly DependencyProperty BotBackgroundProperty = DependencyProperty.Register(
            "BotBackground", typeof(SolidColorBrush), typeof(UserView), new PropertyMetadata(default(SolidColorBrush), RefreshBotBackgroundColor));

        private static void RefreshBotBackgroundColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshBotBackground();
        }

        private void RefreshBotBackground()
        {
            _botScreen.Background = BotBackground;
        }

        public SolidColorBrush BotBackground
        {
            get { return (SolidColorBrush)GetValue(BotBackgroundProperty); }
            set { SetValue(BotBackgroundProperty, value); }
        }
        #endregion

        #region TopCount
        public static readonly DependencyProperty TopCountProperty = DependencyProperty.Register(
            "TopCount", typeof(int), typeof(UserView), new PropertyMetadata(default(int), RefreshTopCount));

        private static void RefreshTopCount(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshTopCount();
        }

        private void RefreshTopCount()
        {
            _topScreen.Count = TopCount;
        }

        public int TopCount
        {
            get { return (int)GetValue(TopCountProperty); }
            set { SetValue(TopCountProperty, value); }
        }
        #endregion

        #region TopOffset
        public static readonly DependencyProperty TopOffsetProperty = DependencyProperty.Register(
            "TopOffset", typeof(int), typeof(UserView), new PropertyMetadata(default(int), RefreshTopOffset));

        private static void RefreshTopOffset(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshTopOffset();
        }

        private void RefreshTopOffset()
        {
            _topScreen.Offset = TopOffset;
        }

        public int TopOffset
        {
            get { return (int)GetValue(TopOffsetProperty); }
            set { SetValue(TopOffsetProperty, value); }
        }
        #endregion

        #region TopIndiagrams
        public static readonly DependencyProperty TopIndiagramsProperty = DependencyProperty.Register(
            "TopIndiagrams", typeof(List<Indiagram>), typeof(UserView), new PropertyMetadata(default(List<Indiagram>), RefreshTopIndiagrams));

        private static void RefreshTopIndiagrams(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshTopIndiagrams();
        }

        private void RefreshTopIndiagrams()
        {
            _topScreen.Indiagrams = TopIndiagrams;
        }


        public List<Indiagram> TopIndiagrams
        {
            get { return (List<Indiagram>)GetValue(TopIndiagramsProperty); }
            set { SetValue(TopIndiagramsProperty, value); }
        }
        #endregion

        #region TopTextColor
        public static readonly DependencyProperty TopTextColorProperty = DependencyProperty.Register(
            "TopTextColor", typeof(SolidColorBrush), typeof(UserView), new PropertyMetadata(default(SolidColorBrush), RefreshTopTextColor));

        private static void RefreshTopTextColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshTopTextColor();
        }

        private void RefreshTopTextColor()
        {
            _topScreen.TextColor = TopTextColor;
        }


        public SolidColorBrush TopTextColor
        {
            get { return (SolidColorBrush)GetValue(TopTextColorProperty); }
            set { SetValue(TopTextColorProperty, value); }
        }
        #endregion

        #region TopIndiagramSelectedCommand
        public static readonly DependencyProperty TopIndiagramSelectedCommandProperty = DependencyProperty.Register(
            "TopIndiagramSelectedCommand", typeof(ICommand), typeof(UserView), new PropertyMetadata(default(ICommand), RefreshTopIndiSelectedCommand));

        private static void RefreshTopIndiSelectedCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshTopIndiaSelectedCommand();
        }

        private void RefreshTopIndiaSelectedCommand()
        {
            _topScreen.IndiagramSelected = TopIndiagramSelectedCommand;
        }

        public ICommand TopIndiagramSelectedCommand
        {
            get { return (ICommand)GetValue(TopIndiagramSelectedCommandProperty); }
            set { SetValue(TopIndiagramSelectedCommandProperty, value); }
        }
        #endregion

        #region BotIndiagramDragStartCommand

        public static readonly DependencyProperty BotIndiagramDragStartCommandProperty = DependencyProperty.Register(
            "BotIndiagramDragStartCommand", typeof(ICommand), typeof(UserView), new PropertyMetadata(default(ICommand), RefreshBotIndiagramDragStartCommand));

        private static void RefreshBotIndiagramDragStartCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshBotIndiagramDragStartCommand();
        }

        private void RefreshBotIndiagramDragStartCommand()
        {
            _botScreen.DragStarCommand = BotIndiagramDragStartCommand;
        }

        public ICommand BotIndiagramDragStartCommand
        {
            get { return (ICommand)GetValue(BotIndiagramDragStartCommandProperty); }
            set { SetValue(BotIndiagramDragStartCommandProperty, value); }
        }

        #endregion

        #region TopNextCommand
        public static readonly DependencyProperty TopNextCommandProperty = DependencyProperty.Register(
            "TopNextCommand", typeof(ICommand), typeof(UserView), new PropertyMetadata(default(ICommand), RefreshTopNextCommand));

        private static void RefreshTopNextCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshTopNextCommand();
        }

        private void RefreshTopNextCommand()
        {
            _topScreen.NextCommand = TopNextCommand;
        }

        public ICommand TopNextCommand
        {
            get { return (ICommand)GetValue(TopNextCommandProperty); }
            set { SetValue(TopNextCommandProperty, value); }
        }
        #endregion

        #region TopNextButtonVisibility
        //todo : a voir pour ça
        public static readonly DependencyProperty TopNextButtonVisibilityProperty = DependencyProperty.Register(
            "TopNextButtonVisibility", typeof(Visibility), typeof(UserView), new PropertyMetadata(default(Visibility)));

        public Visibility TopNextButtonVisibility
        {
            get { return (Visibility)GetValue(TopNextButtonVisibilityProperty); }
            set { SetValue(TopNextButtonVisibilityProperty, value); }
        }
        #endregion

        #region BotIndiagrams
        public static readonly DependencyProperty BotIndiagramsProperty = DependencyProperty.Register(
            "BotIndiagrams", typeof(ObservableCollection<IndiagramUIModel>), typeof(UserView), new PropertyMetadata(default(ObservableCollection<IndiagramUIModel>), RefreshBotIndiagrams));

        private static void RefreshBotIndiagrams(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshBotIndiagrams();
        }

        private void RefreshBotIndiagrams()
        {
            _botScreen.Indiagrams = BotIndiagrams;
        }

        public ObservableCollection<IndiagramUIModel> BotIndiagrams
        {
            get { return (ObservableCollection<IndiagramUIModel>)GetValue(BotIndiagramsProperty); }
            set { SetValue(BotIndiagramsProperty, value); }
        }
        #endregion

        #region BotCanAddIndiagrams
        public static readonly DependencyProperty BotCanAddIndiagramsProperty = DependencyProperty.Register(
            "BotCanAddIndiagrams", typeof(bool), typeof(UserView), new PropertyMetadata(default(bool), RefreshCanAddIndiagram));

        private static void RefreshCanAddIndiagram(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshCanAddIndiagram();
        }

        private void RefreshCanAddIndiagram()
        {
            _botScreen.CanAddIndiagrams = BotCanAddIndiagrams;
        }

        public bool BotCanAddIndiagrams
        {
            get { return (bool)GetValue(BotCanAddIndiagramsProperty); }
            set { SetValue(BotCanAddIndiagramsProperty, value); }
        }
        #endregion

        #region BotReadCommand
        public static readonly DependencyProperty BotReadCommandProperty = DependencyProperty.Register(
            "BotReadCommand", typeof(ICommand), typeof(UserView), new PropertyMetadata(default(ICommand), RefreshBotReadCommand));

        private static void RefreshBotReadCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshBotReadCommand();
        }

        private void RefreshBotReadCommand()
        {
            _botScreen.ReadCommand = BotReadCommand;
        }

        public ICommand BotReadCommand
        {
            get { return (ICommand)GetValue(BotReadCommandProperty); }
            set { SetValue(BotReadCommandProperty, value); }
        }
        #endregion

        #region BotIndiagramSelectedCommand
        public static readonly DependencyProperty BotIndiagramSelectedCommandProperty = DependencyProperty.Register(
            "BotIndiagramSelectedCommand", typeof(ICommand), typeof(UserView), new PropertyMetadata(default(ICommand), RefreshBotIndiagramSelectedCommand));

        private static void RefreshBotIndiagramSelectedCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshBotIndiagramSelectedCommand();
        }

        private void RefreshBotIndiagramSelectedCommand()
        {
            _botScreen.IndiagramSelectedCommand = BotIndiagramSelectedCommand;
        }

        public ICommand BotIndiagramSelectedCommand
        {
            get { return (ICommand)GetValue(BotIndiagramSelectedCommandProperty); }
            set { SetValue(BotIndiagramSelectedCommandProperty, value); }
        }
        #endregion

        #region BotCorrectionCommand
        public static readonly DependencyProperty BotCorrectionCommandProperty = DependencyProperty.Register(
            "BotCorrectionCommand", typeof(ICommand), typeof(UserView), new PropertyMetadata(default(ICommand), RefreshBotCorrectionCommand));

        private static void RefreshBotCorrectionCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as UserView;
            if (view != null) view.RefreshBotCorrectionCommand();
        }

        private void RefreshBotCorrectionCommand()
        {
            _botScreen.CorrectionCommand = BotCorrectionCommand;
        }

        public ICommand BotCorrectionCommand
        {
            get { return (ICommand)GetValue(BotCorrectionCommandProperty); }
            set { SetValue(BotCorrectionCommandProperty, value); }
        }
        #endregion
       
        #endregion

        public UserView()
        {
            Orientation = Orientation.Vertical;
            _botScreen = new SentenceAreaView();
            _topScreen = new IndiagramBrowserView();
            Children.Add(_topScreen);
            Children.Add(_botScreen);
            SizeChanged += UserView_SizeChanged;
            _topScreen.CountChanged += _topScreen_CountChanged;
            _botScreen.CanAddIndiagramsChanged += _botScreen_CanAddIndiagramsChanged;
        }

        #region Callback
        void _botScreen_CanAddIndiagramsChanged(object sender, EventArgs e)
        {
            BotCanAddIndiagrams = _botScreen.CanAddIndiagrams;
        }

        void _topScreen_CountChanged(object sender, EventArgs e)
        {
            TopCount = _topScreen.Count;
        }

        void UserView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var ratio = LazyResolver<ISettingsService>.Service.SelectionAreaHeight;
            var screenHeight = LazyResolver<IScreenService>.Service.Height;
            _topScreen.Height = screenHeight * ratio / 100;
            _botScreen.Height = screenHeight - _topScreen.Height;
        }
        #endregion
    }
}