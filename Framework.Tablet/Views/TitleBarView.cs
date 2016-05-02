using System;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
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
    /// Affiche la barre de titre de la partie utilisateur
    /// </summary>
    public class TitleBarView : Canvas
    {
        /// <summary>
        /// Image de la Categorie courante
        /// </summary>
        private readonly Image _imagecategory;
        /// <summary>
        /// Texte de la Catégorie courante
        /// </summary>
        private readonly TextBlock _textblock;
        /// <summary>
        /// Carré rouge, servant lorsque la Catégorie courante n'a pas d'image
        /// </summary>
        private readonly StackPanel _redRect;

        /// <summary>
        /// Image du bouton back
        /// </summary>
        private readonly Image _buttonBack;

        /// <summary>
        /// Texte de la categorie parente
        /// </summary>
        private readonly TextBlock _oldCategory;
        

        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(
            "Category", typeof(Category), typeof(TitleBarView), new PropertyMetadata(default(Category), Refresh));

        public static readonly DependencyProperty ParentCategoryProperty = DependencyProperty.Register(
            "ParentCategory", typeof(Category), typeof(TitleBarView), new PropertyMetadata(default(Category), RefreshParent));

        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TitleBarView;
            if (view != null) view.Refresh();
        }

        private void Refresh()
        {
            _textblock.Text = Category.Text;
            //pourquoi ne pas sortir la suppression ?
            if (Category.ImagePath == null)
            {
                //si la catégorie n'a pas d'image on met le rectangle rouge
                try
                {
                    Children.RemoveAt(0);
                }
                catch (ArgumentException) { }
                Children.Insert(0, _redRect);
            }
            else
            {
                try
                {
                    Children.RemoveAt(0);
                }
                catch (ArgumentException) { }
                _imagecategory.Source = new BitmapImage(new Uri(Category.ImagePath, UriKind.Absolute));
                Children.Insert(0, _imagecategory);
            }
        }

        private static void RefreshParent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as TitleBarView;
            if (view != null) view.RefreshParent();
        }

        private void RefreshParent()
        {
            _oldCategory.Text = ParentCategory.Text ?? "";
        }

        public Category Category
        {
            get { return (Category) GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public Category ParentCategory
        {
            get { return (Category) GetValue(ParentCategoryProperty); }
            set { SetValue(ParentCategoryProperty, value);}
        }

        #region  BackCommand

        public static readonly DependencyProperty BackCategoryCommandProperty = DependencyProperty.Register(
                   "BackCategoryCommand", typeof(ICommand), typeof(TitleBarView), new PropertyMetadata(default(ICommand)));

        public ICommand BackCategoryCommand
        {
            get { return (ICommand)GetValue(BackCategoryCommandProperty); }
            set { SetValue(BackCategoryCommandProperty, value); }
        }

        private void _backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (BackCategoryCommand != null && BackCategoryCommand.CanExecute(null))
                BackCategoryCommand.Execute(null);
        }
    #endregion

        public TitleBarView()
        {
            var settingsService = LazyResolver<ISettingsService>.Service;
            _textblock = new TextBlock
            {
                FontSize = 20,
                Foreground = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };

            _redRect = new StackPanel
            {
                Height = 60,
                Width = 60,
                Background = new SolidColorBrush(Colors.Red)
            };

            _imagecategory = new Image
            {
                Height = 60,
                Width = 60
            };

            _textblock.Measure(new Size(0, 0));

            Height = 60;
            SetLeft(_textblock, _redRect.Width);
            SetTop(_textblock, (Height - _textblock.ActualHeight) / 2);
            Background = new SolidColorBrush(Colors.White);

            const string sourcelogo = "ms-appx:///Assets/logoIndiaRose.png";
            var logo = new Image
            {
                Source = new BitmapImage(new Uri(sourcelogo)),
                Width = 256
            };
           string sourceBack = LazyResolver<IStorageService>.Service.ImageBackPath;
           _buttonBack = new Image
            {
                Source = new BitmapImage(new Uri(sourceBack)),
                Width = 60,
                Height = 60

            };
            _oldCategory = new TextBlock
            {
                FontSize = 20,
                Foreground = new SolidColorBrush(Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };

            if (!settingsService.IsBackButtonEnabled)
                SetLeft(logo, LazyResolver<IScreenService>.Service.Width - logo.Width);
            else
            {
                SetLeft(logo, (LazyResolver<IScreenService>.Service.Width/2) - (logo.Width/2));
                SetLeft(_buttonBack, LazyResolver<IScreenService>.Service.Width - _buttonBack.Width);

                _oldCategory.Measure(new Size(0, 0));
                Height = 60;
                //TODO Voir a arranger le calcul / enlever la variable magique
                SetLeft(_oldCategory, LazyResolver<IScreenService>.Service.Width - (_buttonBack.Width + _oldCategory.ActualWidth + 100));
                SetTop(_oldCategory, (Height - _oldCategory.ActualHeight) / 2);

                Children.Insert(3, _buttonBack);
                Children.Insert(4, _oldCategory);
            }

            _buttonBack.Tapped += _backButton_Tapped;

            Children.Insert(0, _imagecategory);
            Children.Insert(1, _textblock);
            Children.Insert(2, logo);

        }

        

    }
}
