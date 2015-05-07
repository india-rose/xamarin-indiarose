using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using System;
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
        protected ImageView ImageCategoryView;
        protected TextView TextCategoryView;
        protected Category Category;

        private void Initialize()
        {
            ImageView logo = new ImageView(Context) { Id = 0x0fffff2a };
            logo.SetImageResource(Resource.Drawable.logo);
            logo.SetAdjustViewBounds(true);
            logo.SetMinimumHeight(60);
            logo.SetMaxHeight(120);

            ImageCategoryView = new ImageView(Context);
            ImageCategoryView.SetAdjustViewBounds(true);
            ImageCategoryView.SetMinimumHeight(60);
            ImageCategoryView.SetMaxHeight(60);
            ImageCategoryView.Id = 0x0fffff2b;

            TextCategoryView = new TextView(Context);
            TextCategoryView.SetMaxHeight(60);
            TextCategoryView.SetTextColor(Color.Black);
            TextCategoryView.Id = 0x0fffff2c;
            TextCategoryView.SetTextSize(ComplexUnitType.Sp, 15);
            TextCategoryView.Gravity = GravityFlags.CenterVertical;

            LayoutParams lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentRight);
            lp.AddRule(LayoutRules.CenterVertical);
            AddView(logo, lp);

            lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentRight);
            lp.AddRule(LayoutRules.CenterVertical);
            AddView(ImageCategoryView, lp);

            lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.RightOf, ImageCategoryView.Id);
            lp.AddRule(LayoutRules.CenterVertical);
            lp.SetMargins(60, 0, 60, 0);
            AddView(TextCategoryView, lp);

            SetTitleInfo(new Category()
            {
                ImagePath = LazyResolver<IStorageService>.Service.ImageRootPath,
                Text = "Home"
            });
            Invalidate();
        }

        public void SetTitleInfo(Category category)
        {
            if (!(Category == null || Category.Equals(category)))
            {
                SetCategory(category);
                Category = category;
            }
        }

        protected void SetCategory(Category category)
        {
            if (Category != null)
            {
                if (category.ImagePath != null)
                {
                    ImageCategoryView.SetImageBitmap(BitmapFactory.DecodeFile(category.ImagePath));
                }
                else
                {
                    ImageCategoryView.SetImageDrawable(new ColorDrawable(Color.Red));
                }
                TextCategoryView.Text = Category.Text;
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