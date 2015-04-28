using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using IndiaRose.Business.ViewModels.Admin.Collection;
using IndiaRose.Business.ViewModels.Admin.Collection.Dialogs;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using Environment = Android.OS.Environment;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[BindingElement(Path = "WatchIndiagramCommand", TargetPath = "PositiveButtonEvent")]
	public partial class ExploreCollectionDialog : AlertDialogFragmentBase
	{
		public ExploreCollectionDialog()
		{
			var trad = DependencyService.Container.Resolve<ILocalizationService>();
			Title = trad.GetString("whichActionQuestion", "Text");

			Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
			Buttons.Add(DialogsButton.Neutral, trad.GetString("goIntoText", "Text"));
			Buttons.Add(DialogsButton.Positive, trad.GetString("seeText", "Text"));
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_ExploreCollectionDialog, container, false);
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogsExploreCollectionDialog;
		}
	}
}