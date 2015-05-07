using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using System;
using Android.Content;
using Android.Graphics;

namespace IndiaRose.Framework.Views
{
public class TitleBar : RelativeLayout
{
	protected ImageView ImageCategoryView = null;
	protected TextView TextCategoryView = null;
	protected Category Category = null;

	public TitleBar(Context context) : base(context)
	{
	    ImageView logo = new ImageView(Context) {Id = 0x0fffff2a};
	    logo.SetImageResource(Resource.Drawable.logo);
		logo.SetAdjustViewBounds(true);
		logo.SetMinimumHeight(60);
		logo.SetMaxHeight(120);

        ImageCategoryView = new ImageView(Context);
        ImageCategoryView.SetAdjustViewBounds(true);
        ImageCategoryView.SetMinimumHeight(60);
        ImageCategoryView.SetMaxHeight(60);
        ImageCategoryView.Id=0x0fffff2b;

        TextCategoryView = new TextView(Context);
        TextCategoryView.SetMaxHeight(60);
        TextCategoryView.SetTextColor(Color.Black);
        TextCategoryView.Id=0x0fffff2c;
        TextCategoryView.SetTextSize(ComplexUnitType.Sp,15);
        TextCategoryView.Gravity=GravityFlags.CenterVertical;
        
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
		lp.AddRule(LayoutRules.RightOf,ImageCategoryView.Id);
		lp.AddRule(LayoutRules.CenterVertical);
		lp.SetMargins(60, 0, 60, 0);
		AddView(TextCategoryView, lp);
	}

	public void SetTitleInfo(Category category) 
    {
		if (Category == null || Category.Equals(category)) {
				SetCategory(category);
				Category = category;
		}
	}

	protected void SetCategory(Category category)
    {
		if (Category != null) {
			//ImageCategoryView.SetImageBitmap(BitmapFactory.DecodeFileDescriptor());
			TextCategoryView.Text=Category.Text;
		}
	}
}
}