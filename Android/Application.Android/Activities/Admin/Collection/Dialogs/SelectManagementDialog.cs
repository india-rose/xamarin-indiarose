using Android.App;
using Android.OS;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Application.Activities.Admin.Collection.Dialogs
{
	[Activity(Label = "SelectManagementDialog")]
	public partial class SelectManagementDialog : AlertDialogFragmentBase
	{
		public SelectManagementDialog()
		{
			var trad = DependencyService.Container.Resolve<ILocalizationService>();
			Title = trad.GetString("whichActionQuestion", "Text");

			Buttons.Add(DialogsButton.Negative, trad.GetString("Button_Back", "Text"));
			Buttons.Add(DialogsButton.Neutral, trad.GetString("goIntoText", "Text"));
			Buttons.Add(DialogsButton.Positive, trad.GetString("select", "Text"));
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.Admin_Collection_Dialogs_SelectManagementDialog, container, false);
		}
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminCollectionDialogSelectManagementViewModel;
		}
	}
}