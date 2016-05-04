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
    public class TitleBarView : Grid
    {
        private Grid _tilteBar; 
        /// <summary>
        /// Image de la Categorie courante
        /// </summary>
        private readonly Image _imagecategory;
        /// <summary>
        /// Image du logo
        /// </summary>
        private readonly Image _logo;
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
            "Category", typeof (Category), typeof (TitleBarView), new PropertyMetadata(default(Category), Refresh));

        public static readonly DependencyProperty ParentCategoryProperty = DependencyProperty.Register(
            "ParentCategory", typeof (Category), typeof (TitleBarView),
            new PropertyMetadata(default(Category), RefreshParent));

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
                    _tilteBar.Children.RemoveAt(0);
                }
                catch (ArgumentException)
                {
                }
                _tilteBar.Children.Insert(0, _redRect);
            }
            else
            {
                try
                {;
                    _tilteBar.Children.RemoveAt(0);
                }
                catch (ArgumentException)
                {
                }
                _imagecategory.Source = new BitmapImage(new Uri(Category.ImagePath, UriKind.Absolute));
                _tilteBar.Children.Insert(0, _imagecategory);
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
            set { SetValue(ParentCategoryProperty, value); }
        }

        #region  BackCommand

        public static readonly DependencyProperty BackCategoryCommandProperty = DependencyProperty.Register(
            "BackCategoryCommand", typeof (ICommand), typeof (TitleBarView), new PropertyMetadata(default(ICommand)));

        public ICommand BackCategoryCommand
        {
            get { return (ICommand) GetValue(BackCategoryCommandProperty); }
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
            var screenService = LazyResolver<IScreenService>.Service;

            _tilteBar = new Grid();

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

            Height = 60;

            Background = new SolidColorBrush(Colors.White);

            const string sourcelogo = "ms-appx:///Assets/logoIndiaRose.png";
            _logo = new Image
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
                Margin = new Thickness(0, 0, 10, 0)
            };

            _tilteBar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_imagecategory.Width/*, GridUnitType.Star*/) });


            if (!settingsService.IsBackButtonEnabled)
            {
                _tilteBar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(screenService.Width - _imagecategory.Width, GridUnitType.Star) });
                _logo.HorizontalAlignment = HorizontalAlignment.Right;
                _logo.SetValue(Grid.ColumnProperty, 1);
            }
            else
            {
                _tilteBar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(screenService.Width - (_imagecategory.Width + _buttonBack.Width), GridUnitType.Star) });
                _tilteBar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_buttonBack.Width/*, GridUnitType.Star*/) });

                _logo.SetValue(Grid.ColumnProperty, 1);
                _logo.HorizontalAlignment = HorizontalAlignment.Center;

                _buttonBack.SetValue(Grid.ColumnProperty, 2);
                _oldCategory.HorizontalAlignment = HorizontalAlignment.Right;
                _oldCategory.SetValue(Grid.ColumnProperty, 1);

                _tilteBar.Children.Add(_buttonBack);
                _tilteBar.Children.Add(_oldCategory);
            }

            _buttonBack.Tapped += _backButton_Tapped;
            
            _tilteBar.Children.Insert(0, _imagecategory);

            _textblock.SetValue(Grid.ColumnProperty, 1);
            _textblock.HorizontalAlignment = HorizontalAlignment.Left;
            _tilteBar.Children.Add(_textblock);

            _tilteBar.Children.Add(_logo);

            Children.Insert(0, _tilteBar);
            

        }
       
    }

}
