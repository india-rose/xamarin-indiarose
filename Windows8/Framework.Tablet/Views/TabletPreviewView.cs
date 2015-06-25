﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Android.Support.V4.View;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    public class TabletPreviewView : Grid
    {

        #region Properties

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(
            "ButtonStyle", typeof(Style), typeof(TabletPreviewView), new PropertyMetadata(default(Style), RefreshStyle));

        private static void RefreshStyle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshStyle();
        }

        private void RefreshStyle()
        {
            _topButton.Style = ButtonStyle;
            _bottomButton.Style = ButtonStyle;
        }

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty IndiagramSizeProperty = DependencyProperty.Register(
            "IndiagramSize", typeof(int), typeof(TabletPreviewView), new PropertyMetadata(default(int), RefreshSize));

        public int IndiagramSize
        {
            get { return (int)GetValue(IndiagramSizeProperty); }
            set { SetValue(IndiagramSizeProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register(
            "Percentage", typeof(int), typeof(TabletPreviewView), new PropertyMetadata(default(int), RefreshSize));

        private static void RefreshSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshSize();
        }

        public int Percentage
        {
            get { return (int)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty TopAreaColorProperty = DependencyProperty.Register(
            "TopAreaColor", typeof(SolidColorBrush), typeof(TabletPreviewView), new PropertyMetadata(default(SolidColorBrush), RefreshTopButtonColor));

        private static void RefreshTopButtonColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshTopButtonColor();
        }

        public void RefreshTopButtonColor()
        {
            _topButton.Background = TopAreaColor;
        }

        public SolidColorBrush TopAreaColor
        {
            get { return (SolidColorBrush)GetValue(TopAreaColorProperty); }
            set { SetValue(TopAreaColorProperty, value); }
        }

        public static readonly DependencyProperty BottomAreaColorProperty = DependencyProperty.Register(
            "BottomAreaColor", typeof(SolidColorBrush), typeof(TabletPreviewView), new PropertyMetadata(default(SolidColorBrush), RefreshBotButtonColor));

        private static void RefreshBotButtonColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TabletPreviewView;
            if (view != null) view.RefreshBotButtonColor();
        }

        private void RefreshBotButtonColor()
        {
            _bottomButton.Background = BottomAreaColor;
        }

        public SolidColorBrush BottomAreaColor
        {
            get { return (SolidColorBrush)GetValue(BottomAreaColorProperty); }
            set { SetValue(BottomAreaColorProperty, value); }
        }

        public static readonly DependencyProperty TopAreaCommandProperty = DependencyProperty.Register(
            "TopAreaCommand", typeof(ICommand), typeof(TabletPreviewView), new PropertyMetadata(default(ICommand)));

        public ICommand TopAreaCommand
        {
            get { return (ICommand)GetValue(TopAreaCommandProperty); }
            set { SetValue(TopAreaCommandProperty, value); }
        }

        public static readonly DependencyProperty BottomAreaCommandProperty = DependencyProperty.Register(
            "BottomAreaCommand", typeof(ICommand), typeof(TabletPreviewView), new PropertyMetadata(default(ICommand)));

        public ICommand BottomAreaCommand
        {
            get { return (ICommand)GetValue(BottomAreaCommandProperty); }
            set { SetValue(BottomAreaCommandProperty, value); }
        }
        #endregion

        private readonly Image _tabletImage = new Image();
        private readonly Button _topButton = new Button();
        private readonly Button _bottomButton = new Button();
        private readonly StackPanel _stackPanel = new StackPanel();
        private readonly Grid _topGrid = new Grid();
        private readonly Grid _botGrid = new Grid();
        private readonly Image _nextImage = new Image();
        private readonly Image _chatImage = new Image();
        private readonly Image _lapinImage = new Image();
        private readonly Image _playImage = new Image();

        public TabletPreviewView()
        {
            _chatImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/chat.png"));
            _playImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/playbutton.png"));
            _lapinImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/lapin.png"));
            _nextImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/nextarrow.png"));
            _tabletImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/tab.png"));
            _tabletImage.Stretch = Stretch.Uniform;
            _tabletImage.IsTapEnabled = false;
            _bottomButton.HorizontalAlignment = HorizontalAlignment.Center;
            _bottomButton.Tapped += _bottomButton_Tapped;
            _topButton.HorizontalAlignment = HorizontalAlignment.Center;
            _topButton.Tapped += _topButton_Tapped;
            _stackPanel.Orientation = Orientation.Vertical;


            Children.Add(_tabletImage);
            Children.Add(_stackPanel);
            _stackPanel.Children.Add(_topGrid);
            _stackPanel.Children.Add(_botGrid);

            SizeChanged += TabletPreview_SizeChanged;
        }

        void _bottomButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (BottomAreaCommand != null && BottomAreaCommand.CanExecute(null))
            {
                BottomAreaCommand.Execute(null);
            }
        }

        void _topButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TopAreaCommand != null && TopAreaCommand.CanExecute(null))
            {
                TopAreaCommand.Execute(null);
            }
        }

        void TabletPreview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshSize();
        }

        private void RefreshSize()
        {
            var height = _tabletImage.ActualHeight;
            var width = _tabletImage.ActualWidth;
            height *= 0.81;
            width *= 0.79;
            _stackPanel.Height = height;
            _stackPanel.Width = width;

            var ratiorealscreen = height / LazyResolver<IScreenService>.Service.Height;
            var indiasize = IndiagramSize * ratiorealscreen;
            indiasize *= 1.2;
            RefreshIndiaSize(indiasize);
            var nbIndia = (int)(width/indiasize);

            _botGrid.ColumnDefinitions.Clear();
            _topGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < nbIndia; i++)
            {
                _botGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                _topGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            try
            {
                AddGrid(nbIndia);
            }
            catch (ArgumentException e)
            {
            }

            _topButton.Height = height * (Percentage / 100.0);
            _topButton.Width = width;
            _bottomButton.Width = width;
            _bottomButton.Height = height * (1 - (Percentage / 100.0));
        }

        private void RefreshIndiaSize(double indiasize)
        {
            _playImage.Height = indiasize;
            _nextImage.Height = indiasize;
            _lapinImage.Height = indiasize;
            _chatImage.Height = indiasize;

            _playImage.Width = indiasize;
            _nextImage.Width = indiasize;
            _lapinImage.Width = indiasize;
            _chatImage.Width = indiasize;

        }

        private void AddGrid(int nbIndia)
        {
            SetColumnSpan(_bottomButton, nbIndia);
            SetColumnSpan(_topButton, nbIndia);
            if (nbIndia > 1)
                SetColumn(_chatImage, 0);
            if (nbIndia > 2)
                SetColumn(_lapinImage, 1);
            if (nbIndia > 0)
            {
                SetColumn(_nextImage, nbIndia - 1);
                SetColumn(_playImage, nbIndia - 1);
            }

            _topGrid.Children.Clear();
            _botGrid.Children.Clear();

            _topGrid.Children.Add(_topButton);
            _topGrid.Children.Add(_lapinImage);
            _topGrid.Children.Add(_chatImage);
            _topGrid.Children.Add(_nextImage);

            _botGrid.Children.Add(_bottomButton);
            _botGrid.Children.Add(_playImage);


        }
    }
}
