using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using IndiaRose.Data.Model;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class UserView : StackPanel
    {
        private readonly SentenceAreaView _botScreen;
        private readonly IndiagramBrowserView _topScreen;

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

        public static readonly DependencyProperty BotBackgroundProperty = DependencyProperty.Register(
            "BotBackground", typeof (SolidColorBrush), typeof (UserView), new PropertyMetadata(default(SolidColorBrush), RefreshBotBackgroundColor));

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
            get { return (SolidColorBrush) GetValue(BotBackgroundProperty); }
            set { SetValue(BotBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TopCountProperty = DependencyProperty.Register(
            "TopCount", typeof (int), typeof (UserView), new PropertyMetadata(default(int)));

        public int TopCount
        {
            get { return (int) GetValue(TopCountProperty); }
            set { SetValue(TopCountProperty, value); }
        }

        public static readonly DependencyProperty TopOffsetProperty = DependencyProperty.Register(
            "TopOffset", typeof (int), typeof (UserView), new PropertyMetadata(default(int)));

        public int TopOffset
        {
            get { return (int) GetValue(TopOffsetProperty); }
            set { SetValue(TopOffsetProperty, value); }
        }

        public static readonly DependencyProperty TopIndiagramsProperty = DependencyProperty.Register(
            "TopIndiagrams", typeof (List<Indiagram>), typeof (UserView), new PropertyMetadata(default(List<Indiagram>)));

        public List<Indiagram> TopIndiagrams
        {
            get { return (List<Indiagram>) GetValue(TopIndiagramsProperty); }
            set { SetValue(TopIndiagramsProperty, value); }
        }

        public static readonly DependencyProperty TopTextColorProperty = DependencyProperty.Register(
            "TopTextColor", typeof (SolidColorBrush), typeof (UserView), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush TopTextColor
        {
            get { return (SolidColorBrush) GetValue(TopTextColorProperty); }
            set { SetValue(TopTextColorProperty, value); }
        }

        public static readonly DependencyProperty TopIndiagramSelectedCommandProperty = DependencyProperty.Register(
            "TopIndiagramSelectedCommand", typeof (ICommand), typeof (UserView), new PropertyMetadata(default(ICommand)));

        public ICommand TopIndiagramSelectedCommand
        {
            get { return (ICommand) GetValue(TopIndiagramSelectedCommandProperty); }
            set { SetValue(TopIndiagramSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty TopNextCommandProperty = DependencyProperty.Register(
            "TopNextCommand", typeof (ICommand), typeof (UserView), new PropertyMetadata(default(ICommand)));

        public ICommand TopNextCommand
        {
            get { return (ICommand) GetValue(TopNextCommandProperty); }
            set { SetValue(TopNextCommandProperty, value); }
        }

        public static readonly DependencyProperty TopNextButtonVisibilityProperty = DependencyProperty.Register(
            "TopNextButtonVisibility", typeof (Visibility), typeof (UserView), new PropertyMetadata(default(Visibility)));

        public Visibility TopNextButtonVisibility
        {
            get { return (Visibility) GetValue(TopNextButtonVisibilityProperty); }
            set { SetValue(TopNextButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty BotIndiagramsProperty = DependencyProperty.Register(
            "BotIndiagrams", typeof (ObservableCollection<IndiagramUIModel>), typeof (UserView), new PropertyMetadata(default(ObservableCollection<IndiagramUIModel>)));

        public ObservableCollection<IndiagramUIModel> BotIndiagrams
        {
            get { return (ObservableCollection<IndiagramUIModel>) GetValue(BotIndiagramsProperty); }
            set { SetValue(BotIndiagramsProperty, value); }
        }

        public static readonly DependencyProperty BotCanAddIndiagramsProperty = DependencyProperty.Register(
            "BotCanAddIndiagrams", typeof (bool), typeof (UserView), new PropertyMetadata(default(bool)));

        public bool BotCanAddIndiagrams
        {
            get { return (bool) GetValue(BotCanAddIndiagramsProperty); }
            set { SetValue(BotCanAddIndiagramsProperty, value); }
        }

        public static readonly DependencyProperty BotReadCommandProperty = DependencyProperty.Register(
            "BotReadCommand", typeof (ICommand), typeof (UserView), new PropertyMetadata(default(ICommand)));

        public ICommand BotReadCommand
        {
            get { return (ICommand) GetValue(BotReadCommandProperty); }
            set { SetValue(BotReadCommandProperty, value); }
        }

        public static readonly DependencyProperty BotIndiagramSelectedCommandProperty = DependencyProperty.Register(
            "BotIndiagramSelectedCommand", typeof (ICommand), typeof (UserView), new PropertyMetadata(default(ICommand)));

        public ICommand BotIndiagramSelectedCommand
        {
            get { return (ICommand) GetValue(BotIndiagramSelectedCommandProperty); }
            set { SetValue(BotIndiagramSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty BotCorrectionCommandProperty = DependencyProperty.Register(
            "BotCorrectionCommand", typeof (ICommand), typeof (UserView), new PropertyMetadata(default(ICommand)));

        public ICommand BotCorrectionCommand
        {
            get { return (ICommand) GetValue(BotCorrectionCommandProperty); }
            set { SetValue(BotCorrectionCommandProperty, value); }
        }
        public UserView()
        {
            Orientation = Orientation.Vertical;
            _botScreen=new SentenceAreaView();
            _topScreen=new IndiagramBrowserView();
            Children.Add(_topScreen);
            Children.Add(_botScreen);
            SizeChanged += UserView_SizeChanged;
        }

        void UserView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var ratio = LazyResolver<ISettingsService>.Service.SelectionAreaHeight;
            _topScreen.Height = ActualHeight*ratio/100;
            _botScreen.Height = ActualHeight - _topScreen.Height;
        }
    }
}
