﻿using System;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace IndiaRose.Framework.Views
{
    /// <summary>
    /// View préfabriqué de la barre de titre de la page utilisateur
    /// </summary>
    public class TitleBarView : RelativeLayout
    {
        private ImageView _imageCategoryView;
        private TextView _textCategoryView;
        private Category _category;

        public Category Category
        {
            get { return _category; }
            set
            {
                SetTitleInfo(value);
            }
        }

        private void Initialize()
        {
            //Initialisation du logo India Rose
            ImageView logo = new ImageView(Context) { Id = 0x0fffff2a };
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

            LayoutParams lp = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentRight);
            lp.AddRule(LayoutRules.CenterVertical);
            AddView(logo, lp);

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