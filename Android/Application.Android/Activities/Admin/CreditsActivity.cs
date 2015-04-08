#region Usings

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Storm.Mvvm;

#endregion

namespace IndiaRose.Application.Activities.Admin
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape, Theme = "@style/Theme.Sherlock.Light.NoActionBar")]
	public partial class CreditsActivity : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Admin_CreditsPage);
			//TODO : ajouter le viewmodel (même si presque vide, il sera nécessaire pour les autres plateformes)
		}
	}
}