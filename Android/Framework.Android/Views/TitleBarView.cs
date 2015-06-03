using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using System;
using System.Runtime.Remoting.Contexts;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
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
            ImageView logo = new ImageView(Context) { Id = 0x0fffff2a };
            logo.SetImageResource(Resource.Drawable.logo);
            logo.SetAdjustViewBounds(true);
            logo.SetMinimumHeight(60);
            logo.SetMaxHeight(60);

            _imageCategoryView = new ImageView(Context);
            _imageCategoryView.SetAdjustViewBounds(true);
            _imageCategoryView.SetMinimumHeight(60);
            _imageCategoryView.SetMaxHeight(60);
            _imageCategoryView.Id = 0x0fffff2b;

            _textCategoryView = new TextView(Context);
            _textCategoryView.SetMaxHeight(60);
            _textCategoryView.SetTextColor(Color.Black);
            _textCategoryView.Id = 0x0fffff2c;
            _textCategoryView.SetTextSize(ComplexUnitType.Sp, 15);
            _textCategoryView.Gravity = GravityFlags.CenterVertical;

            LayoutParams lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentRight);
            lp.AddRule(LayoutRules.CenterVertical);
            AddView(logo, lp);

            lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentLeft);
            lp.AddRule(LayoutRules.CenterVertical);
            AddView(_imageCategoryView, lp);

            lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.RightOf, _imageCategoryView.Id);
            lp.AddRule(LayoutRules.CenterVertical);
            lp.SetMargins(60, 0, 60, 0);
            AddView(_textCategoryView, lp);
        }

        public void SetTitleInfo(Category category)
        {
            if (category!=null && !category.Equals(Category))
            {
                SetCategory(category);
                _category = category;
            }
        }

        protected void SetCategory(Category category)
        {
            if (category != null)
            {
                if (category.ImagePath != null)
                {
                    _imageCategoryView.SetImageBitmap(BitmapFactory.DecodeFile(category.ImagePath));
                }
                else
                {
                    _imageCategoryView.SetImageDrawable(new ColorDrawable(Color.Red));
                }
                _textCategoryView.Text = category.Text;
            }
        }
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
    }
}