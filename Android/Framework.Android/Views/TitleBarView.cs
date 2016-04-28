using System;
using System.Windows.Input;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
    /// <summary>
    /// View préfabriqué de la barre de titre de la page utilisateur
    /// </summary>
    public class TitleBarView : RelativeLayout
    {
        private ImageView _imageCategoryView;
        private ImageView _backButton;
        private TextView _textCategoryView;
        private TextView _oldCategory;
        private Category _category;
        private Category _parent;

        protected IStorageService StorageService => LazyResolver<IStorageService>.Service;

        protected ISettingsService SettingsService => LazyResolver<ISettingsService>.Service;

        #region Private tools methods

        private bool SetProperty<T>(ref T storage, T value)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            return true;
        }

        private bool SetProperty<T>(ref T storage, T value, Func<EventHandler> eventGetter)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;

            EventHandler handler = eventGetter();
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            return true;
        }
    #endregion

        public Category Category
        {
            get { return _category; }
            set
            {
                SetTitleInfo(value);
            }
        }

        public Category ParentCategory
        {
            get { return _parent; }
            set { SetOldCategoryText(value); }
        }

        /// <summary>
        /// Commande lorsque le bouton backCategory (currentCategory) est sélectionné
        /// </summary>
		public ICommand BackCategoryCommand { get;set ; }

        private void Initialize()
        {
            //Initialisation du logo India Rose
            ImageView logo = new ImageView(Context) {Id = 0x0fffff2a};
            logo.SetImageResource(Resource.Drawable.logo);
            logo.SetAdjustViewBounds(true);
            logo.SetMinimumHeight(60);
            logo.SetMaxHeight(60);

            //Initialisation de l'image de la catégorie courante
            _imageCategoryView = new ImageView(Context);
            _imageCategoryView.SetAdjustViewBounds(true);
            _imageCategoryView.SetMinimumHeight(60);
            _imageCategoryView.SetMaxHeight(60);
            _imageCategoryView.Id = 0x0fffff2b;
            _imageCategoryView.SetMinimumWidth(60);
            _imageCategoryView.SetMaxWidth(60);
            _imageCategoryView.Measure(60, 60);

            //Initialisation du texte de la catégorie courante
            _textCategoryView = new TextView(Context);
            _textCategoryView.SetMaxHeight(60);
            _textCategoryView.SetTextColor(Color.Black);
            _textCategoryView.Id = 0x0fffff2c;
            _textCategoryView.SetTextSize(ComplexUnitType.Sp, 15);
            _textCategoryView.Gravity = GravityFlags.CenterVertical;

            LayoutParams lp;
            _oldCategory = new TextView(Context);

            if (SettingsService.IsBackButtonEnabled)
            {
                //Initialisation du bouton retour
                _backButton = new ImageView(Context);
                _backButton.SetAdjustViewBounds(true);
                _backButton.SetMinimumHeight(60);
                _backButton.SetMaxHeight(60);
                _backButton.Id = 0x0fffff2d;
                _backButton.SetMinimumWidth(60);
                _backButton.SetMaxWidth(60);
                _backButton.Measure(60, 60);
                _backButton.SetImageBitmap(BitmapFactory.DecodeFile(StorageService.ImageBackPath));
                //Defini en tant que bouton backCategory
                _backButton.Touch += OnBackCategoryTouch;

                //Initialisation du texte de la catégorie precedente
                _oldCategory.SetMaxHeight(60);
                _oldCategory.SetTextColor(Color.Black);
                _oldCategory.Id = 0x0fffff2e;
                _oldCategory.SetTextSize(ComplexUnitType.Sp, 15);
                _oldCategory.Gravity = GravityFlags.CenterVertical;

                lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lp.AddRule(LayoutRules.CenterInParent);
                lp.AddRule(LayoutRules.CenterVertical);
                AddView(logo, lp);

                lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lp.AddRule(LayoutRules.AlignParentRight);
                lp.AddRule(LayoutRules.CenterVertical);
                AddView(_backButton, lp);

                lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lp.AddRule(LayoutRules.LeftOf, _backButton.Id);
                lp.AddRule(LayoutRules.CenterVertical);
                lp.SetMargins(60, 0, 60, 0);
                AddView(_oldCategory, lp);
            }
            else
            {
                lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lp.AddRule(LayoutRules.AlignParentRight);
                lp.AddRule(LayoutRules.CenterVertical);
                AddView(logo, lp);
            }
            lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentLeft);
            lp.AddRule(LayoutRules.CenterVertical);
            AddView(_imageCategoryView, lp);

            lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.RightOf, _imageCategoryView.Id);
            lp.AddRule(LayoutRules.CenterVertical);
            lp.SetMargins(60, 0, 60, 0);
            AddView(_textCategoryView, lp);
        }

        /// <summary>
        /// Change la barre de titre (image et texte) avec la nouvelle catégorie
        /// </summary>
        /// <param name="category">Nouvelle catégorie</param>
        public void SetTitleInfo(Category category)
        {
            if (category != null && !category.Equals(Category))
            {
                if (category.ImagePath != null)
                {
                    try
                    {
                        _imageCategoryView.SetImageBitmap(BitmapFactory.DecodeFile(category.ImagePath));
                    }
                    catch (Exception)
                    {
                        //TODO : log error
                    }

                    LayoutParams param = (LayoutParams)_textCategoryView.LayoutParameters;
                    param.SetMargins(60, 0, 60, 0);
                    _textCategoryView.LayoutParameters = param;
                }
                else
                {
                    LayoutParams param = (LayoutParams)_textCategoryView.LayoutParameters;
                    param.SetMargins(120, 0, 60, 0);
                    _textCategoryView.LayoutParameters = param;
                    _imageCategoryView.SetImageDrawable(new ColorDrawable(Color.Red));
                }
                _textCategoryView.Text = category.Text;
                _category = category;
            }
        }

        /// <summary>
        /// Callback lorsque le bouton backCategory (currentCategory) est sélectionné
        /// </summary>
		private void OnBackCategoryTouch(object sender, TouchEventArgs touchEventArgs)
        {
            if (touchEventArgs.Event.ActionMasked == MotionEventActions.Down)
            {
                ICommand command = BackCategoryCommand;
                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }

        private void SetOldCategoryText(Category c)
        {
            _oldCategory.Text = c.Text;
            _parent = c;
        }

        #region Constructeurs
        public TitleBarView(Context context)
            : base(context)
        {
            Initialize();
        }

        public TitleBarView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public TitleBarView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }
        #endregion
    }
}